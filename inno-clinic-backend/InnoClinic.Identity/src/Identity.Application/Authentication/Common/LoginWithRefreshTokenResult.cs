namespace Identity.Application.Authentication.Common;

public record LoginWithRefreshTokenResult(
    string Token,
    string RefreshToken);