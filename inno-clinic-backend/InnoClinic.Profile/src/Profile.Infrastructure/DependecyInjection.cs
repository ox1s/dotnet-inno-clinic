using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Profile.Infrastructure.Auth;
using Profile.Infrastructure.Database;
using Profile.Infrastructure.Database.Repositories;

namespace Profile.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        AddPersistence(services, configuration);
        return services;
    }
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("innoclinic-database") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<ProfileDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<AccountRepository>();
        services.AddScoped<DoctorRepository>();
        services.AddScoped<PatientRepository>();
        services.AddScoped<ReceptionistRepository>();
    }
}