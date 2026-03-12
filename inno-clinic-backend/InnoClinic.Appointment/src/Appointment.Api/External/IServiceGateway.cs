using Appointment.Api.Common;

namespace Appointment.Api.External;

public interface IServiceGateway
{
    Task<Result<bool>> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default);
    Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default);
}