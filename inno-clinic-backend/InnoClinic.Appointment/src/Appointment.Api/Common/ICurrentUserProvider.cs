namespace Appointment.Api.Common;

public interface ICurrentUserProvider
{
    Guid? GetUserId();
    string? GetUserRole();
}
