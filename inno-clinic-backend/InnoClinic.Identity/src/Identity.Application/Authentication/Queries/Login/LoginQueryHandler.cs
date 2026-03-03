using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;

using MediatR;

using Microsoft.Extensions.Logging;

using Serilog;

namespace Identity.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IAccountsRepository accountsRepository,
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork,
    IProfileService profileService,
    ILogger<LoginQueryHandler> logger)
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

        // TODO: HARD CODED ROLE, пока передаю в запросе, так как нет профиля и ролей
        // var profileResult = await profileService.GetProfileDataAsync(account.Id, cancellationToken);
        var profileResult = query.HardCodedRole switch
        {
            Roles.Patient => ErrorOrFactory.From((Role: Roles.Patient, Status: "Active")),
            Roles.Doctor => ErrorOrFactory.From((Role: Roles.Doctor, Status: "Active")),
            Roles.Receptionist => ErrorOrFactory.From((Role: Roles.Receptionist, Status: "Active")),
            _ => ErrorOrFactory.From((Role: Roles.Patient, Status: "Active"))
        };
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

        Log.Information("User {Email} logged in", email);

        return new LoginResult(jwtTokenGenerator.GenerateToken(account, role), refreshToken.Token);
    }
}