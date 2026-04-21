using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

using Profile.Infrastructure.Auth;

namespace Profile.Api.Extensions;

public static class HostDiExtensions
{
    public static IServiceCollection AddWebHostInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();

        services.AddAuth(configuration);

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

        services.AddSingleton<IAuthorizationHandler, BotApiKeyHandler>();

        services.AddAuthorization(
            options =>
            {
                options.AddPolicy("BotPolicy", policy =>
                {
                    policy.Requirements.Add(new BotApiKeyRequirement());
                    policy.AuthenticationSchemes.Clear();
                });
            }
        );

        return services;
    }
}