using System.Text;
using System.Text.Json;

using InnoClinic.Notification.Email;
using InnoClinic.Notification.Entities;
using InnoClinic.Shared.Contracts.Notifications;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Notification.Consumers;

public class EmailVerificationConsumer(
    IConnection connection,
    EmailSender emailSender,
    IOptions<EmailSettings> emailOptions,
    ILogger<EmailVerificationConsumer> logger,
    IMongoClient mongoDb) : BackgroundService
{
    private IChannel? _channel;
    private readonly EmailSettings _emailSettings = emailOptions.Value;
    private readonly IMongoCollection<Account> _accountsCollection =
        mongoDb.GetDatabase("notifications-db").GetCollection<Account>("accounts");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: "email-verification-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var command = JsonSerializer.Deserialize<SendVerificationEmailCommand>(message);
                logger.LogInformation("Received request to send email to: {Email}", command?.Email);
                if (command is null) throw new ArgumentNullException(nameof(command));

                await emailSender.SendEmailAsync(
                    command.Email,
                    _emailSettings.FromEmail,
                    _emailSettings.WelcomeSubject,
                    string.Format(_emailSettings.WelcomeBodyTemplate, command?.VerificationLink)
                );

                var filter = Builders<Account>.Filter.Eq(x => x.Id, command.AccountId);
                var update = Builders<Account>.Update
                    .Set(x => x.Email, command.Email)
                    .SetOnInsert(x => x.Id, command.AccountId);

                await _accountsCollection.UpdateOneAsync(
                    filter,
                    update,
                    new UpdateOptions { IsUpsert = true },
                    cancellationToken: stoppingToken);

                logger.LogInformation("Account {AccountId} synced to MongoDB", command.AccountId);

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message");
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: "email-verification-queue",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}