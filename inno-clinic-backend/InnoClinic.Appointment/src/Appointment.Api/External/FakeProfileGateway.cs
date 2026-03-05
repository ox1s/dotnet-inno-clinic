namespace Appointment.Api.External;

public class FakeProfileGateway : IProfileGateway
{
    public Task<bool> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}