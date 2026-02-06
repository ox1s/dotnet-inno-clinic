using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common;

using InnoClinic.Shared;

using MediatR;

namespace Identity.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IAccountsRepository accountsRepository,
    IProfileService profileService)
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(query.Email);
        if (emailResult.IsError) return emailResult.Errors;
        var email = emailResult.Value;

        var account = await accountsRepository.GetByEmailAsync(email, cancellationToken);
        if (account == null
            || !account.IsCorrectPasswordHash(query.Password, passwordHasher))
            return AuthenticationErrors.InvalidCredentials;

        var profileResult = await profileService.GetProfileDataAsync(account.Id, cancellationToken);
        if (profileResult.IsError) return AuthenticationErrors.InvalidCredentials;

        var (role, status) = profileResult.Value;
        if (role != Roles.Patient && status == "Inactive") return AccountErrors.AccountInactive;

        return new AuthenticationResult(account, jwtTokenGenerator.GenerateToken(account, role));
    }
}