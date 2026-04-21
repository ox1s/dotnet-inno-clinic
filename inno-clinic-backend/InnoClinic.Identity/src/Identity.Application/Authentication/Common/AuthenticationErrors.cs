using ErrorOr;

namespace Identity.Application.Authentication.Common;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials = Error.Validation(
        code: "Authentication.InvalidCredentials",
        description: "Invalid credentials");

    public static readonly Error InvalidRefreshToken = Error.Validation(
        code: "Authentication.InvalidRefreshToken",
        description: "Invalid refresh token");

    public static readonly Error UserRoleNotFound = Error.Validation(
        code: "Authentication.UserRoleNotFound",
        description: "User role not found");

    public static readonly Error ProfileNotFound = Error.Validation(
        code: "Authentication.ProfileNotFound",
        description: "Profile not found");
}