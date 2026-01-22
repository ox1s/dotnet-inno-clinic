using Appointment.Api.Data;
using FluentValidation;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;

using Appointment.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
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
    builder.Services.AddEndpoints();

    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    builder.Services.AddDbContext<AppointmentDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("innoclinic-database"))
    );

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
        dbContext.Database.Migrate();
    }

    app.MapEndpoints();

    app.Run();
}



