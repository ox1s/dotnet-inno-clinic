using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
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
        if (account == null)
            return AuthenticationErrors.InvalidCredentials;

        var isPasswordValid = account.IsCorrectPasswordHash(query.Password, passwordHasher);
        if (!isPasswordValid)
        {
            logger.LogInformation("Incorrect password for login attempt for email {Email}", email);
            return AuthenticationErrors.InvalidCredentials;
        }

        // ? Если правильные кредишинлы, мы пускаем хотя бы как пациента, если ляжет Profile
        // Если Profile недоступен, worker сможет залогиниться, но не получит своих приемуществ. Возможно надо 
        // чтобы инфомация о профиле бралась с Identity API, а не с ProfileAPI
        var (role, status) = (Roles.Patient, string.Empty);
        var profileResult = await profileService.GetProfileDataAsync(account.Id, cancellationToken);
        
        if (!profileResult.IsError)
        {
            (role, status) = profileResult.Value;
        }

        if (!IsRoleAllowedForLogin(role, status))
        {
            return AccountErrors.AccountInactive;
        }

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

    private static bool IsRoleAllowedForLogin(string role, string status)
    {
        if (string.Equals(role, Roles.Patient, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(status, "Inactive", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (string.Equals(role, Roles.Doctor, StringComparison.OrdinalIgnoreCase))
        {
            return status is Statuses.AtWork
                or Statuses.OnVacation
                or Statuses.SickDay
                or Statuses.SickLeave
                or Statuses.SelfIsolation
                or Statuses.LeaveWithoutPay;
        }

        if (string.Equals(role, Roles.Receptionist, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}
