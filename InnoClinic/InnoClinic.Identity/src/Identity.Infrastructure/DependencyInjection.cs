using System.Net.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Identity.Application.Common.Interfaces;
using Identity.Domain.Common;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Security.PasswordHasher;
using Identity.Infrastructure.Security.TokenGenerator;
using Identity.Infrastructure.Services.Email;
using Identity.Infrastructure.Services;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(configuration)
            .AddHttpContextAccessor()
            .AddMediatR()
            .AddConfigurations(configuration)

            .AddServices(configuration)
            .AddPersistence(configuration)
            .AddHealthChecks(configuration);

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        return services;
    }
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        return services;
    }
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("innoclinic-accounts");

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString)); ;

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IdentityDbContext>());
        services.AddScoped<IAccountsRepository, AccountsRepository>();

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("innoclinic-accounts")!);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();

        var mailPitConnectionString = configuration.GetConnectionString("mailpit");

        if (string.IsNullOrEmpty(mailPitConnectionString))
        {
            throw new InvalidOperationException("MailPit connection string is missing");
        }

        mailPitConnectionString = mailPitConnectionString.Replace("Endpoint=", "");

        var uri = new Uri(mailPitConnectionString, UriKind.Absolute);

        var host = uri.Host;
        var port = uri.Port;

        var emailSettings = configuration.GetSection(EmailSettings.Section).Get<EmailSettings>() ?? new EmailSettings();

        services
            .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
            .AddSmtpSender(new SmtpClient(host, port));

        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }
}
