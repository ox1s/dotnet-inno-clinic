using System.Text;
using System.Text.Json;

using InnoClinic.Notification.Email;
using InnoClinic.Notification.Entities;
using InnoClinic.Shared.DTOs;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Notification.Consumers;

public class SendDailyPollCommandConsumer(
    IConnection connection,
    EmailSender emailSender,
    IOptions<EmailSettings> emailOptions,
    ILogger<SendDailyPollCommandConsumer> logger,
    IMongoClient mongoDb)
    : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly IMongoCollection<Account> _accountsCollection =
        mongoDb.GetDatabase("notifications-db").GetCollection<Account>("accounts");

    private IChannel? _channel;
    private readonly EmailSettings _emailSettings = emailOptions.Value;
    private const string ExchangeName = "notifications";
    private const string QueueName = "email-notifications-queue";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(
                    queue: QueueName,
                    exchange: ExchangeName,
                    routingKey: string.Empty,
                    arguments: null,
                    cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var command = JsonSerializer.Deserialize<SendDailyPollCommand>(message, JsonOptions);

                if (command != null)
                {
                    logger.LogInformation("Received request to send daily poll email: {Commmand}", command);

                    var account = await _accountsCollection.Find(x => x.Id == command.AccountId).FirstOrDefaultAsync();
                    if (account == null || string.IsNullOrEmpty(account.Email))
                    {
                        logger.LogWarning("Account with Id {AccountId} not found in MongoDB. Skipping email.", command.AccountId);
                        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                        return;
                    }

                    logger.LogInformation("This email exist in mongoDb, so email will be sent to: {Email} with {Id}", account.Email, account.Id);
                    await SendEmailAsync(command.AccountId, account.Email, command.Message, command.TelegramLink);
                }

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task SendEmailAsync(
        Guid accountId,
        string email,
        string message,
        string link)
    {
        logger.LogInformation("Begin sending email to: {Email} with {AccountId}", email, accountId);

        await emailSender.SendEmailAsync(
            email,
            _emailSettings.FromEmail,
            "Daily Poll",
            message + string.Format(_emailSettings.PollBodyTemplate, link)
        );

        logger.LogInformation("------------------------------------------------------");
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}