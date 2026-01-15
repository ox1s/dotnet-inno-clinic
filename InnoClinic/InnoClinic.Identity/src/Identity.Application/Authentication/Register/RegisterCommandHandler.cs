using ErrorOr;
using MediatR;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.Common;
using Identity.Domain.AccountAggregate;

namespace Identity.Application.Authentication.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IAccountsRepository accountsRepository)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsError) return emailResult.Errors;

        var email = emailResult.Value;
        // TODO: Check if account email is already registered

        var hashPasswordResult = passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError) return hashPasswordResult.Errors;

        var account = Account.Create(
            email: email,
            passwordHash: hashPasswordResult.Value);

        await accountsRepository.AddAccountAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(account);

        // TODO: Email verification (generate link & send email)

        return new AuthenticationResult(
            account,
            token);
    }
}
