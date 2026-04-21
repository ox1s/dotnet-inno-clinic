using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;

using MediatR;

namespace Identity.Application.Authentication.Queries.LoginWithRefreshToken;

public class LoginWithRefreshTokenHandler(
    IRefreshTokensRepository refreshTokensRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IAccountsRepository accountsRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork)
    : IRequestHandler
        <LoginWithRefreshTokenQuery,
        ErrorOr<LoginWithRefreshTokenResult>>
{
    public async Task<ErrorOr<LoginWithRefreshTokenResult>> Handle(
        LoginWithRefreshTokenQuery query,
        CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokensRepository
            .GetRefreshTokenAsync(query.RefreshToken, cancellationToken);
        if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            return AuthenticationErrors.InvalidRefreshToken;

        var account = await accountsRepository
            .GetByIdAsync(refreshToken.AccountId, cancellationToken);
        if (account == null) return AuthenticationErrors.InvalidRefreshToken;

        var role = userContext.UserRole;

        var accessToken = jwtTokenGenerator.GenerateToken(account, role);

        refreshToken.Token = jwtTokenGenerator.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new LoginWithRefreshTokenResult(accessToken, refreshToken.Token);
    }
}