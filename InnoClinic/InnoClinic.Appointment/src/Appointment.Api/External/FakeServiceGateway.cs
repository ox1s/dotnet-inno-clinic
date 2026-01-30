using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public class FakeServiceGateway : IServiceGateway
{
    public Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<ServiceDto>> GetServicesAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        var services = ids.Select(id => new ServiceDto(id, "Medical Service", true));
        return Task.FromResult(services);
    }

    public Task<TimeSpan?> GetServiceDurationAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<TimeSpan?>(TimeSpan.FromMinutes(30));
    }
}
