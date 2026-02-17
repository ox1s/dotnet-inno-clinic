using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public interface IServiceGateway
{
    Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default);
}