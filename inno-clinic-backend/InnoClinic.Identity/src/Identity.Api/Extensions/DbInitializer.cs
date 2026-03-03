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

        var emailResult = Email.Create(receptionistEmail);
        if (emailResult.IsError)
        {
            logger.LogError(
                "Identity seed skipped: receptionist email '{ReceptionistEmail}' is invalid: {Reason}",
                receptionistEmail,
                emailResult.FirstError.Description);
            return;
        }

        var email = emailResult.Value;
        if (await accountsRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            logger.LogInformation("Identity seed skipped: receptionist '{ReceptionistEmail}' already exists", receptionistEmail);
            return;
        }

        var account = Account.Create(
            email,
            passwordHasher.HashPassword(receptionistPassword),
            dateTimeProvider);

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
            "Identity seed completed: receptionist '{ReceptionistEmail}' was created",
            receptionistEmail);
    }
}
