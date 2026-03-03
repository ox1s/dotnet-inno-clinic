using ErrorOr;

using Identity.Application.Common.Interfaces;
using Identity.Application.Common.Settings;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;

using MediatR;

using Microsoft.Extensions.Options;

namespace Identity.Application.Authentication.Commands.CreateWorkerAccount;

public class CreateWorkerAccountCommandHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IAccountsRepository accountsRepository,
    IEmailSender emailSender,
    IEmailVerificationLinkFactory linkFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<EmailSettings> emailSettingsOptions)
    : IRequestHandler<CreateWorkerAccountCommand, ErrorOr<CreateWorkerAccountResult>>
{
    private readonly EmailSettings _emailSettings = emailSettingsOptions.Value;

    public async Task<ErrorOr<CreateWorkerAccountResult>> Handle(
        CreateWorkerAccountCommand command,
        CancellationToken cancellationToken)
    {
        var normalizedRole = NormalizeWorkerRole(command.Role);
        if (normalizedRole is null)
        {
            return Error.Validation(
                code: "WorkerAccount.InvalidRole",
                description: $"Role must be either {Roles.Doctor} or {Roles.Receptionist}.");
        }

        var emailResult = Email.Create(command.Email);
        if (emailResult.IsError)
        {
            return emailResult.Errors;
        }

        var email = emailResult.Value;
        if (await accountsRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            return AccountErrors.AlreadyExists;
        }

        var hashPassword = passwordHasher.HashPassword(command.Password);
        var account = Account.Create(email, hashPassword, dateTimeProvider);

        await accountsRepository.AddAccountAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        if (account.EmailVerificationToken is null)
        {
            return Error.Failure(
                code: "Account.TokenMissing",
                description: "Verification token generation failed.");
        }

        var verificationLink = linkFactory.Create(account.Id, account.EmailVerificationToken);
        await emailSender.SendEmailAsync(
            account.Email.Value,
            _emailSettings.FromEmail,
            _emailSettings.WelcomeSubject,
            string.Format(_emailSettings.WelcomeBodyTemplate, verificationLink));

        return new CreateWorkerAccountResult(account.Id, account.Email.Value, normalizedRole);
    }

    private static string? NormalizeWorkerRole(string role)
    {
        if (string.Equals(role, Roles.Doctor, StringComparison.OrdinalIgnoreCase))
        {
            return Roles.Doctor;
        }

        if (string.Equals(role, Roles.Receptionist, StringComparison.OrdinalIgnoreCase))
        {
            return Roles.Receptionist;
        }

        return null;
    }
}