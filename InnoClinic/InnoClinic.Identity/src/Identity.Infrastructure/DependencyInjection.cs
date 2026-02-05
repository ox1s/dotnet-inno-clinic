using System.Net.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Identity.Application.Common.Interfaces;
using Identity.Application.Common.Settings;
using Identity.Domain.Common;
using Identity.Domain.Common.Interfaces;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security.PasswordHasher;
using Identity.Infrastructure.Security.TokenGenerator;
using Identity.Infrastructure.Services.Email;
using Identity.Infrastructure.Services.Profile;
using Identity.Infrastructure.Services.Time;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services
                .AddAuthentication()
                .AddHttpContextAccessor()
                .AddMediatR()
                .AddConfigurations(configuration)

                .AddServices(configuration)
                .AddPersistence(configuration)
                .AddHealthChecks(configuration);

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
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
            var emailSettings =  configuration.GetSection(EmailSettings.Section).Get<EmailSettings>();

            if (emailSettings is null)
            {
                throw new InvalidOperationException("Email settings are missing");
            }

            services.AddTransient<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
            services.AddScoped<IProfileService, FakeProfileService>();

            var mailPitConnectionString = configuration.GetConnectionString("mailpit");

            if (string.IsNullOrEmpty(mailPitConnectionString))
            {
                throw new InvalidOperationException("MailPit connection string is missing");
            }

            mailPitConnectionString = mailPitConnectionString.Replace("Endpoint=", "");

            var uri = new Uri(mailPitConnectionString, UriKind.Absolute);

            var host = uri.Host;
            var port = uri.Port;

            services
                .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
                .AddSmtpSender(new SmtpClient(host, port));

            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }

        private IServiceCollection AddAuthentication()
        {
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}
