using System.Text;

using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;
using ClinicManagement.Api.Extensions;
using ClinicManagement.Api.Features.Offices;
using ClinicManagement.Api.Features.Services;
using ClinicManagement.Api.Features.Specializations;
using ClinicManagement.Api.Feautures.Categories;
using ClinicManagement.Api.Feautures.Offices;
using ClinicManagement.Api.Feautures.Services;
using ClinicManagement.Api.Feautures.Specializations;
using ClinicManagement.Api.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(configure =>
    configure.CustomizeProblemDetails = context =>
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier));

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

builder.Services.AddScoped<CreateService>();
builder.Services.AddScoped<UpdateService>();
builder.Services.AddScoped<ListServices>();
builder.Services.AddScoped<CheckService>();
builder.Services.AddScoped<GetService>();
builder.Services.AddScoped<ListActiveServicesByCategory>();
builder.Services.AddScoped<ChangeServiceStatus>();
builder.Services.AddScoped<GetServiceDurationMinutes>();

builder.Services.AddScoped<ListCategories>();

builder.Services.AddScoped<CreateSpecialization>();
builder.Services.AddScoped<DeleteSpecialization>();
builder.Services.AddScoped<ListSpecializations>();
builder.Services.AddScoped<GetSpecialization>();
builder.Services.AddScoped<UpdateSpecialization>();
builder.Services.AddScoped<ChangeSpecializationStatus>();

builder.Services.AddScoped<CreateOffice>();
builder.Services.AddScoped<DeleteOffice>();
builder.Services.AddScoped<UpdateOffice>();
builder.Services.AddScoped<CheckOffice>();
builder.Services.AddScoped<ListOffices>();
builder.Services.AddScoped<GetOffice>();
builder.Services.AddScoped<ChangeOfficeStatus>();

builder.Services.AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

await app.InitializeAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.UseExceptionHandler();
await app.RunAsync();
