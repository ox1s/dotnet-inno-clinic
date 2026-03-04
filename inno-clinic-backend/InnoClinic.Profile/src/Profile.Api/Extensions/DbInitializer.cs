using Microsoft.EntityFrameworkCore;

using Profile.Domain.Entities.AccountProfiles;
using Profile.Domain.Entities.Receptionists;
using Profile.Infrastructure.Database;

namespace Profile.Api.Extensions;

public static class DbInitializer
{
    private const string DefaultReceptionistAccountId = "817dfc4f-f275-484d-a47e-f461f54f02e4";
    private const string DefaultReceptionistOfficeId = "3d4574d2-885e-4506-a31f-957ff40f6420";
    private const string DefaultReceptionistFirstName = "Default";
    private const string DefaultReceptionistLastName = "Receptionist";
    private const string DefaultReceptionistMiddleName = "Seed";

    public static async Task InitializeAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");
        var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        var receptionistAccountIdRaw =
            configuration["DbInitializer:Receptionist:AccountId"] ?? DefaultReceptionistAccountId;
        var receptionistOfficeIdRaw =
            configuration["DbInitializer:Receptionist:OfficeId"] ?? DefaultReceptionistOfficeId;
        var receptionistFirstName =
            configuration["DbInitializer:Receptionist:FirstName"] ?? DefaultReceptionistFirstName;
        var receptionistLastName =
            configuration["DbInitializer:Receptionist:LastName"] ?? DefaultReceptionistLastName;
        var receptionistMiddleName =
            configuration["DbInitializer:Receptionist:MiddleName"] ?? DefaultReceptionistMiddleName;

        if (!Guid.TryParse(receptionistAccountIdRaw, out var receptionistAccountId))
        {
            logger.LogError(
                "Profile seed skipped: receptionist account id '{ReceptionistAccountId}' is invalid",
                receptionistAccountIdRaw);
            return;
        }

        if (!Guid.TryParse(receptionistOfficeIdRaw, out var receptionistOfficeId))
        {
            logger.LogError(
                "Profile seed skipped: receptionist office id '{ReceptionistOfficeId}' is invalid",
                receptionistOfficeIdRaw);
            return;
        }

        if (string.IsNullOrWhiteSpace(receptionistFirstName)
            || string.IsNullOrWhiteSpace(receptionistLastName)
            || string.IsNullOrWhiteSpace(receptionistMiddleName))
        {
            logger.LogError("Profile seed skipped: receptionist full name must not be empty");
            return;
        }

        var exists = await dbContext.Set<Receptionist>()
            .AsNoTracking()
            .AnyAsync(r => r.AccountId == receptionistAccountId, cancellationToken);

        if (exists)
        {
            logger.LogInformation(
                "Profile seed skipped: receptionist with account id '{ReceptionistAccountId}' already exists",
                receptionistAccountId);
            return;
        }

        var receptionist = Receptionist.Create(
            FirstName.Create(receptionistFirstName),
            LastName.Create(receptionistLastName),
            MiddleName.Create(receptionistMiddleName),
            receptionistAccountId,
            receptionistOfficeId);

        await dbContext.Set<Receptionist>().AddAsync(receptionist, cancellationToken);
        await dbContext.CommitChangesAsync(cancellationToken);

        logger.LogInformation(
            "Profile seed completed: receptionist with account id '{ReceptionistAccountId}' was created",
            receptionistAccountId);
    }
}
