using ErrorOr;

using Identity.Application.Authentication.Common;

using MediatR;

namespace Identity.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password)
    : IRequest<ErrorOr<LoginResult>>;