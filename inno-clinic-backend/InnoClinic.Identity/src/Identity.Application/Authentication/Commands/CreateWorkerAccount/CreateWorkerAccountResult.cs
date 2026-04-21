namespace Identity.Application.Authentication.Commands.CreateWorkerAccount;

public record CreateWorkerAccountResult(
    Guid AccountId,
    string Email,
    string Role);