namespace Appointment.Api.External;

public class FakeServiceGateway : IServiceGateway
{
    public Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
    public Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<TimeSpan?>(TimeSpan.FromMinutes(30));
    }
}