namespace Appointment.Api.External;

public interface IProfileGateway
{
    Task<bool> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default);
}