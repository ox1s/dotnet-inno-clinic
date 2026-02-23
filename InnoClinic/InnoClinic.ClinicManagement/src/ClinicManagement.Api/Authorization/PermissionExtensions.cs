using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Api.Authorization;

public static class PermissionExtensions
{
    public static void RequirePermission(
        this AuthorizationPolicyBuilder builder,
        params string[] allowedPermissions)
    {
        builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
    }
    public static RouteHandlerBuilder RequirePermission(
        this RouteHandlerBuilder builder,
        params string[] allowedPermissions)
    {
        return builder.RequireAuthorization(policy =>
            policy.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions)));
    }
}

