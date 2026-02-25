namespace Appointment.Api.External;

public class FakeOfficeGateway : IOfficeGateway
{
    public Task<bool> IsOfficeActiveAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}