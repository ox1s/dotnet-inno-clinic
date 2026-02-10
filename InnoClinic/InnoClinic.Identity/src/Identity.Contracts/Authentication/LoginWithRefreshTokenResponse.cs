namespace Identity.Contracts.Authentication;

public record LoginWithRefreshTokenResponse(
    string Token,
    string RefreshToken);