namespace Appointment.Api.External;

public interface IServiceGateway
{
    Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default);
    Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default);
}