using Appointment.Api.Common;

namespace Appointment.Api.External;

public interface IOfficeGateway
{
    Task<Result<bool>> IsOfficeActiveAsync(Guid officeId, CancellationToken cancellationToken = default);
}