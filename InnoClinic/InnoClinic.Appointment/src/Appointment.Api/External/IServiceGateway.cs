using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public interface IServiceGateway
{
    Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ServiceDto>> GetServicesAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default);
}
