namespace Appointment.Api.Common;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public Guid? GetUserId()
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value
                          ?? httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public string? GetUserRole()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst("role")?.Value;
    }
}
