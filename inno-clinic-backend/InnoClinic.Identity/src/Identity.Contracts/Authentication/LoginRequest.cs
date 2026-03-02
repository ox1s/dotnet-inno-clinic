namespace Identity.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password,
    string HardCodedRole = "Patient");