using System.Text;

using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;
using ClinicManagement.Api.Extensions;
using ClinicManagement.Api.ServiceCategories;
using ClinicManagement.Api.Services;
using ClinicManagement.Api.Specializations;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});
builder.AddSeqEndpoint("seq");

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddEndpointsApiExplorer();
builder.
    Services.AddSwaggerGen(options =>
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

        options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    });

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("innoclinic-database")));

builder.Services.AddHttpContextAccessor();

builder.Services
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

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CreateService>();
builder.Services.AddScoped<UpdateService>();
builder.Services.AddScoped<ListServices>();
builder.Services.AddScoped<GetService>();
builder.Services.AddScoped<CreateServiceCategory>();
builder.Services.AddScoped<CreateSpecialization>();

builder.Services.AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

//app.MapEndpoints();
app.MapEndpoint<CreateService.Endpoint>();
app.MapEndpoint<ListServices.Endpoint>();
app.MapEndpoint<GetService.Endpoint>();
app.MapEndpoint<UpdateService.Endpoint>();
app.MapEndpoint<CreateServiceCategory.Endpoint>();
app.MapEndpoint<CreateSpecialization.Endpoint>();

app.UseExceptionHandler();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();