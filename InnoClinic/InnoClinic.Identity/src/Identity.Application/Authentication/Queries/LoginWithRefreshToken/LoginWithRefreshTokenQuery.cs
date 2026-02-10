using ErrorOr;

using MediatR;

using Identity.Application.Authentication.Common;

namespace Identity.Application.Authentication.Queries.LoginWithRefreshToken;

public record LoginWithRefreshTokenQuery(
    string RefreshToken)
    : IRequest<ErrorOr<LoginWithRefreshTokenResult>>;