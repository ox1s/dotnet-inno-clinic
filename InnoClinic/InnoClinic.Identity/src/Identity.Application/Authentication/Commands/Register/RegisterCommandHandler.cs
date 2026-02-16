using ErrorOr;

using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Application.Common.Settings;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;

using InnoClinic.Shared;

using MediatR;

using Microsoft.Extensions.Options;

namespace Identity.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IAccountsRepository accountsRepository,
    IEmailSender emailSender,
    IEmailVerificationLinkFactory linkFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<EmailSettings> emailSettingsOptions)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly EmailSettings _emailSettings = emailSettingsOptions.Value;
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

        await emailSender.SendEmailAsync(
            account.Email.Value,
            _emailSettings.FromEmail,
            _emailSettings.WelcomeSubject,
            string.Format(_emailSettings.WelcomeBodyTemplate, verificationLink)
        );


        var token = jwtTokenGenerator.GenerateToken(account, Roles.Patient);

        return new AuthenticationResult(
            account,
            token);
    }
}