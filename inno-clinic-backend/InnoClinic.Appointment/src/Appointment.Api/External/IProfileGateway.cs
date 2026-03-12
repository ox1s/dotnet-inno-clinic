using Appointment.Api.Common;

namespace Appointment.Api.External;

public interface IProfileGateway
{
    Task<Result<bool>> IsDoctorActiveAsync(Guid doctorId, CancellationToken cancellationToken = default);
}