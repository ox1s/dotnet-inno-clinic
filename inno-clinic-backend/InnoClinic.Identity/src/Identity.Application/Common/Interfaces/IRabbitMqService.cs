using InnoClinic.Shared.Contracts.Notifications;

namespace Identity.Application.Common.Interfaces;

public interface IRabbitMqService
{
    Task PublishAsync(SendVerificationEmailCommand command);
}