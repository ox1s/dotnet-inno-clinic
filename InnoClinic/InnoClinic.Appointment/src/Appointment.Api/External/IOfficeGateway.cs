namespace Appointment.Api.External;

public interface IOfficeGateway
{
    Task<bool> IsOfficeActiveAsync(Guid officeId, CancellationToken cancellationToken = default);
}