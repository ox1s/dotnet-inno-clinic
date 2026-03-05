using System.Text;
using System.Text.Json;

using Identity.Application.Common.Interfaces;

using InnoClinic.Shared.Contracts.Notifications;

using RabbitMQ.Client;

using Serilog;

namespace Identity.Infrastructure.Services.Bus;

public class RabbitMqService(IConnection connection) : IRabbitMqService
{
    public async Task PublishAsync(SendVerificationEmailCommand command)
    {
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            "email-verification-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = false
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "email-verification-queue",
            mandatory: false,
            basicProperties: properties,
            body: new ReadOnlyMemory<byte>(body));

        Log.Information("Processed item of type {Type} : {Command}", typeof(SendVerificationEmailCommand), command);
    }
}