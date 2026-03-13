using Microsoft.OpenApi;

namespace Identity.Api.Extensions;

public static class HostDiExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        services.AddAuth();
        services.AddProblemDetails();

        return services;
    }
    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecuritySchemeReference("Bearer"),
                new List<string>()
            }
            });
        });
        services.AddAuthorization();

        return services;
    }
}