using ErrorOr;
using Identity.Application.Authentication.Common;
using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common;
using MediatR;

namespace Identity.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IAccountsRepository accountsRepository,
    IEmailSender emailSender,
    IEmailVerificationLinkFactory linkFactory)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsError) return emailResult.Errors;

        var email = emailResult.Value;
        if (await accountsRepository.ExistsByEmailAsync(email, cancellationToken))
            return AccountErrors.AlreadyExists;

        var hashPasswordResult = passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError) return hashPasswordResult.Errors;

        var account = Account.Create(
            email: email,
            passwordHash: hashPasswordResult.Value);

        await accountsRepository.AddAccountAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        var verificationLink = linkFactory.Create(account.Id, account.EmailVerificationToken!);

        await emailSender.SendEmailAsync(
            account.Email.Value,
            "noreply@innoclinic.com",
            "Welcome to InnoClinic! Please verify your email.",
            $"Hello! Click here to verify: <a href='{verificationLink}'>Verify Email</a>"
        );

        // Если чет случилось надо проверить то, что юзер не добавился, а то сейчас он добавляется

        var token = jwtTokenGenerator.GenerateToken(account);

        return new AuthenticationResult(
            account,
            token);
    }
}
