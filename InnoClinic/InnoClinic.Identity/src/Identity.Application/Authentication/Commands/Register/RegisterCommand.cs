using ErrorOr;
using Identity.Application.Authentication.Common;
using MediatR;

namespace Identity.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
