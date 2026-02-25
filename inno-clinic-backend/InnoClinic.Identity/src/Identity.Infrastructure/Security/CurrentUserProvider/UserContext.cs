using System.Security.Claims;

using Identity.Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Identity.Infrastructure.Security.CurrentUserProvider;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        Guid.TryParse(GetSingleClaimValue("id"), out var parsedUserId) ?
            parsedUserId :
            throw new UserContextUnavailableException();

    public string UserRole =>
        GetSingleClaimValue(ClaimTypes.Role)
        ?? throw new UserContextUnavailableException();


    private string GetSingleClaimValue(string claimType) =>
        httpContextAccessor.HttpContext!.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;

    private class UserContextUnavailableException : Exception
    {
        public UserContextUnavailableException() : base("User context is unavailable") { }
    }
}