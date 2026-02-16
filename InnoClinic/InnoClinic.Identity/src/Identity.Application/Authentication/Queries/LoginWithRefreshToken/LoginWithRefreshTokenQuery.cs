using ErrorOr;

using Identity.Application.Authentication.Common;

using MediatR;

namespace Identity.Application.Authentication.Queries.LoginWithRefreshToken;

public record LoginWithRefreshTokenQuery(
    string RefreshToken)
    : IRequest<ErrorOr<LoginWithRefreshTokenResult>>;