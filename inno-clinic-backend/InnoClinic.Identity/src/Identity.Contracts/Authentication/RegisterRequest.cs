namespace Identity.Contracts.Authentication;

public record RegisterRequest(
    string Email,
    string Password,
    string HardCodedRole = "Patient");