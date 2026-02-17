using InnoClinic.Shared.DTOs;

namespace Appointment.Api.External;

public class FakeServiceGateway : IServiceGateway
{
    public Task<bool> IsServiceActiveAsync(Guid serviceId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}