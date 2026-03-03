using ErrorOr;

using MediatR;

namespace Identity.Application.Authentication.Commands.CreateWorkerAccount;

public record CreateWorkerAccountCommand(
    string Email,
    string Password,
    string Role)
    : IRequest<ErrorOr<CreateWorkerAccountResult>>;