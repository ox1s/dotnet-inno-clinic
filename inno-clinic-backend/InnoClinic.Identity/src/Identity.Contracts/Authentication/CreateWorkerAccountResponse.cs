namespace Identity.Contracts.Authentication;

public record CreateWorkerAccountResponse(
    Guid AccountId,
    string Email,
    string Role);