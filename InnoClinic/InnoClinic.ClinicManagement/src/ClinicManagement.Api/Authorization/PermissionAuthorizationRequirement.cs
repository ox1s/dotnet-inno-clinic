using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Api.Authorization;

public class PermissionAuthorizationRequirement(params string[] allowedPermissions)
    : AuthorizationHandler<PermissionAuthorizationRequirement>, IAuthorizationRequirement
{
    public string[] AllowedPermissions { get; } = allowedPermissions;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value);

        var userPermissions = new HashSet<string>();
        foreach (var role in userRoles)
        {
            if (RolePermissionMapping.Map.TryGetValue(role, out var permissionsForRole))
            {
                userPermissions.UnionWith(permissionsForRole);
            }
        }

        if (requirement.AllowedPermissions.Any(userPermissions.Contains))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

