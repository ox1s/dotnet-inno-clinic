using System.Text;
using System.Text.Json;

using InnoClinic.Notification.Entities;
using InnoClinic.Shared.DTOs;

using MongoDB.Driver;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Notification.Consumers;

public class DoctorCreatedConsumer(
    IConnection connection,
    ILogger<DoctorCreatedConsumer> logger,
    IMongoClient mongoDb)
    : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private IChannel? _channel;
    private readonly IMongoCollection<Account> _accountsCollection =
        mongoDb.GetDatabase("notifications-db").GetCollection<Account>("accounts");
    private const string ExchangeName = "doctor-events";
    private const string QueueName = "telegram-notifications-queue";
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
                var command = JsonSerializer.Deserialize<DoctorCreated>(message, JsonOptions);
                if (command != null)
                {
                    logger.LogInformation("Received DoctorCreated event for AccountId: {AccountId} and Email: {Email}", command.AccountId, command.Email);
                    await SaveToMongoAsync(command, stoppingToken);
                }

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing DoctorCreated message");
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task SaveToMongoAsync(
        DoctorCreated message,
        CancellationToken ct)
    {
        logger.LogInformation("Begin syncing AccountId: {AccountId}", message.AccountId);

        var filter = Builders<Account>.Filter.Eq(x => x.Id, message.AccountId);
        var update = Builders<Account>.Update
            .Set(x => x.Email, message.Email)
            .SetOnInsert(x => x.Id, message.AccountId);

        await _accountsCollection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken: ct);

        logger.LogInformation("Finished syncing AccountId: {AccountId}", message.AccountId);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}
