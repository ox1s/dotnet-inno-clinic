using System.Text;
using System.Text.Json;

using InnoClinic.Notification.Email;
using InnoClinic.Shared.DTOs;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Notification.Consumers;

public class EmailVerificationConsumer(
    IConnection connection,
    EmailSender emailSender,
    IOptions<EmailSettings> emailOptions,
    ILogger<EmailVerificationConsumer> logger)
    : BackgroundService
{
    private IChannel? _channel;
    private readonly EmailSettings _emailSettings = emailOptions.Value;
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

                if (command != null)
                {
                    logger.LogInformation("Received request to send email to: {Email}", command.Email);
                    await SendEmailAsync(command);
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
            queue: "email-verification-queue",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task SendEmailAsync(
        SendVerificationEmailCommand command)
    {
        logger.LogInformation("Begin sending email to: {AccountId}", command.AccountId);

        await emailSender.SendEmailAsync(
            command.Email,
            _emailSettings.FromEmail,
            _emailSettings.WelcomeSubject,
            string.Format(_emailSettings.WelcomeBodyTemplate, command.VerificationLink)
        );

        logger.LogInformation("Email sent to: {AccountId}", command.AccountId);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}