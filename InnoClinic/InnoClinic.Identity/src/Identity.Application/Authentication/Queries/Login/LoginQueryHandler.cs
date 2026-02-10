using ErrorOr;

using MediatR;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;

namespace Identity.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IAccountsRepository accountsRepository,
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork,
    IProfileService profileService)
    : IRequestHandler
        <LoginQuery,
        ErrorOr<LoginResult>>
{
    public async Task<ErrorOr<LoginResult>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(query.Email);
        if (emailResult.IsError) return emailResult.Errors;
        var email = emailResult.Value;

        var account = await accountsRepository.GetByEmailAsync(email, cancellationToken);
        if (account == null
            || !account.IsCorrectPasswordHash(query.Password, passwordHasher))
            return AuthenticationErrors.InvalidCredentials;

        // TODO: Реализовать через CurrentUserProvider
        var profileResult = await profileService.GetProfileDataAsync(account.Id, cancellationToken);
        if (profileResult.IsError) return AuthenticationErrors.InvalidCredentials;

        var (role, status) = profileResult.Value;
        if (role != Roles.Patient && status == "Inactive") return AccountErrors.AccountInactive;

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Token = jwtTokenGenerator.GenerateRefreshToken(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7)
        };

        await refreshTokensRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new LoginResult(jwtTokenGenerator.GenerateToken(account, role), refreshToken.Token);
    }
}