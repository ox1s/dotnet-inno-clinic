namespace Identity.Contracts.Authentication;

public record LoginResponse(
    string Token,
    string RefreshToken);