using System.Text;

using Appointment.Api.Data;
using Appointment.Api.External;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Appointment.Api.Extensions;

public static class HostDiExtensions
{
    public static IServiceCollection AddWebHostInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddAuth(configuration);

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<IProfileGateway, ProfileGateway>();
        services.AddScoped<IServiceGateway, ServiceGateway>();
        services.AddScoped<IOfficeGateway, OfficeGateway>();

        services
            .AddEfCore(configuration, environment);

        return services;
    }
    private static IServiceCollection AddEfCore(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("innoclinic-database");

        if (!environment.IsEnvironment("Testing"))
        {
            services.AddDbContext<AppointmentDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
        }
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();

        services
        .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });

        services.AddAuthorization();

        return services;
    }
}