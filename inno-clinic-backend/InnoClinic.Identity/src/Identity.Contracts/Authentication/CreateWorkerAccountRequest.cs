namespace Identity.Contracts.Authentication;

public record CreateWorkerAccountRequest(
    string Email,
    string Password,
    string Role);
