using Identity.Application.Common.Interfaces;
using Identity.Domain.Common.Interfaces;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security.CurrentUserProvider;
using Identity.Infrastructure.Security.PasswordHasher;
using Identity.Infrastructure.Security.TokenGenerator;
using Identity.Infrastructure.Security.TokenValidation;
using Identity.Infrastructure.Services.Bus;
using Identity.Infrastructure.Services.Email;
using Identity.Infrastructure.Services.Profile;
using Identity.Infrastructure.Services.Time;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services
                .AddHttpContextAccessor()
                .AddMediatR()
                .AddConfigurations(configuration)
                .AddServices(configuration)
                .AddPersistence(configuration)
                .AddHealthChecks(configuration)
                .AddAuthentication()
                .AddAuthorization();

            return services;
        }

        private IServiceCollection AddMediatR()
        {
            services.AddMediatR(options =>
                options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

            return services;
        }

        private IServiceCollection AddConfigurations(IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            return services;
        }

        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("innoclinic-database");

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "identity");
                }));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

            // Scrutter для регистрации
            // DateTimeOffset

            return services;
        }

        private IServiceCollection AddHealthChecks(IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("innoclinic-database")!);

            return services;
        }

        private IServiceCollection AddServices(IConfiguration configuration)
        {
            services.AddHttpClient<IProfileService, ProfileService>(client =>
            {
                client.BaseAddress = new Uri("http://profile-api");
            });
            
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

            services.AddTransient<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();

            services.AddScoped<IRabbitMqService, RabbitMqService>();

            return services;
        }

        private IServiceCollection AddAuthentication()
        {
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.ConfigureOptions<JwtBearerTokenValidationConfiguration>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserContext, UserContext>();

            services
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}