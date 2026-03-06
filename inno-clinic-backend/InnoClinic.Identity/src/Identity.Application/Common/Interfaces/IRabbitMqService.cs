using InnoClinic.Shared.DTOs;

namespace Identity.Application.Common.Interfaces;

public interface IRabbitMqService
{
    Task PublishAsync(SendVerificationEmailCommand command);
}