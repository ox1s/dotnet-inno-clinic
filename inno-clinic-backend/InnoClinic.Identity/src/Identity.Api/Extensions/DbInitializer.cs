using Identity.Application.Common.Interfaces;
using Identity.Domain.AccountAggregate;
using Identity.Domain.Common.Interfaces;
using Identity.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Extensions;

public static class DbInitializer
{
    private const string DefaultReceptionistEmail = "receptionist@innoclinic.com";
    private const string DefaultReceptionistPassword = "RecepT1!";
    private const string DefaultReceptionistAccountId = "817dfc4f-f275-484d-a47e-f461f54f02e4";

    public static async Task InitializeAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var accountsRepository = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        await dbContext.Database.MigrateAsync(cancellationToken);

        var receptionistEmail =
            configuration["DbInitializer:Receptionist:Email"] ?? DefaultReceptionistEmail;
        var receptionistPassword =
            configuration["DbInitializer:Receptionist:Password"] ?? DefaultReceptionistPassword;
        var receptionistAccountIdRaw =
            configuration["DbInitializer:Receptionist:AccountId"] ?? DefaultReceptionistAccountId;

        var emailResult = Email.Create(receptionistEmail);
        if (emailResult.IsError)
        {
            logger.LogError(
                "Identity seed skipped: receptionist email '{ReceptionistEmail}' is invalid: {Reason}",
                receptionistEmail,
                emailResult.FirstError.Description);
            return;
        }

        if (!Guid.TryParse(receptionistAccountIdRaw, out var receptionistAccountId))
        {
            logger.LogError(
                "Identity seed skipped: receptionist account id '{ReceptionistAccountId}' is invalid",
                receptionistAccountIdRaw);
            return;
        }

        var email = emailResult.Value;
        var existingAccount = await accountsRepository.GetByEmailAsync(email, cancellationToken);
        if (existingAccount is not null)
        {
            if (existingAccount.Id != receptionistAccountId)
            {
                logger.LogWarning(
                    "Identity seed warning: receptionist '{ReceptionistEmail}' already exists with account id '{ExistingReceptionistAccountId}', expected '{ConfiguredReceptionistAccountId}'. Profile seed may not match until ids are aligned.",
                    receptionistEmail,
                    existingAccount.Id,
                    receptionistAccountId);
            }

            logger.LogInformation(
                "Identity seed skipped: receptionist '{ReceptionistEmail}' already exists with account id '{ReceptionistAccountId}'",
                receptionistEmail,
                existingAccount.Id);
            return;
        }

        var account = Account.Create(
            email,
            passwordHasher.HashPassword(receptionistPassword),
            dateTimeProvider,
            receptionistAccountId);

        if (account.EmailVerificationToken is { } emailVerificationToken)
        {
            var verifyResult = account.VerifyEmail(emailVerificationToken, dateTimeProvider);
            if (verifyResult.IsError)
            {
                logger.LogWarning(
                    "Receptionist '{ReceptionistEmail}' created but email verification was not applied: {Reason}",
                    receptionistEmail,
                    verifyResult.FirstError.Description);
            }
        }

        await accountsRepository.AddAccountAsync(account, cancellationToken);
        await unitOfWork.CommitChangesAsync(cancellationToken);

        logger.LogInformation(
            "Identity seed completed: receptionist '{ReceptionistEmail}' was created with account id '{ReceptionistAccountId}'",
            receptionistEmail,
            receptionistAccountId);
    }
}