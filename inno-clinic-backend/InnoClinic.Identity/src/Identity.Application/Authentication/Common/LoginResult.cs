namespace Identity.Application.Authentication.Common;

public record LoginResult(
    string Token,
    string RefreshToken);