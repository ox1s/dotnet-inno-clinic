using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;
using InnoClinic.Shared.Contracts.Notifications;

using MediatR;

namespace Identity.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IAccountsRepository accountsRepository,
    IEmailVerificationLinkFactory linkFactory,
    IDateTimeProvider dateTimeProvider,
    IRabbitMqService rabbitMqService)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsError) return emailResult.Errors;

        var email = emailResult.Value;
        if (await accountsRepository.ExistsByEmailAsync(email, cancellationToken))
            return AccountErrors.AlreadyExists;

        var hashPassword = passwordHasher.HashPassword(command.Password);

        var account = Account.Create(
            email,
            hashPassword,
            dateTimeProvider);

        await accountsRepository.AddAccountAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        if (account.EmailVerificationToken is null)
            return Error.Failure(
                "Account.TokenMissing",
                "Verification token generation failed.");

        var verificationLink = linkFactory.Create(account.Id, account.EmailVerificationToken!);

        await rabbitMqService.PublishAsync(new SendVerificationEmailCommand(
            account.Id,
            account.Email.Value,
            verificationLink
        ));

        var token = jwtTokenGenerator.GenerateToken(account, Roles.Patient);

        return new AuthenticationResult(
            account,
            token);
    }
}