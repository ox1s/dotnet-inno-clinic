using ErrorOr;
using MediatR;

using Identity.Application.Authentication.Common;

namespace Identity.Application.Authentication.Register;

public record RegisterCommand(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
