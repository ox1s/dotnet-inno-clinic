using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Persistence;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this IHost app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;

        try
        {
            IdentityDbContext context = services.GetRequiredService<IdentityDbContext>();
            ILogger<IdentityDbContext> logger = services.GetRequiredService<ILogger<IdentityDbContext>>();

            logger.LogInformation("Applying database migrations...");

            await context.Database.MigrateAsync();

            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            ILogger<IdentityDbContext> errorLogger = services.GetRequiredService<ILogger<IdentityDbContext>>();
            errorLogger.LogError(ex, "An error occurred while applying database migrations.");
            throw;
        }
    }
}
