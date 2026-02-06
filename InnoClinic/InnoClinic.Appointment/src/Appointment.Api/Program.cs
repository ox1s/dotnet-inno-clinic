using System.Text;

using Appointment.Api.Data;
using Appointment.Api.Endpoints;
using Appointment.Api.External;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
{

    var services = builder.Services;
    services.AddEndpointsApiExplorer();

    services
        .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
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
    services.AddEndpoints();

    services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    if (!builder.Environment.IsEnvironment("Testing"))
    {
        services.AddDbContext<AppointmentDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("innoclinic-database"))
        );
    }
    services.AddScoped<IProfileGateway, FakeProfileGateway>();
    services.AddScoped<IServiceGateway, FakeServiceGateway>();
    services.AddScoped<IOfficeGateway, FakeOfficeGateway>();
}
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider
            .GetRequiredService<AppointmentDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapEndpoints();

    app.Run();
}

public partial class Program { }