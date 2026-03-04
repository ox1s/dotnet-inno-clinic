using Wolverine;

using Profile.Infrastructure;
using Profile.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Profile.Api.Extensions;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    var configuration = builder.Configuration;
    var environment = builder.Environment;
    var logging = builder.Logging;
    var host = builder.Host;

    services.AddEndpointsApiExplorer();
    services.AddHttpClient();

    host.UseWolverine();

    services.AddHealthChecks();

    services
            .AddWebHostInfrastructure(configuration, environment)
            .AddInfrastructure(builder.Configuration);
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
            .GetRequiredService<ProfileDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultEndpoints();

    app.MapPost("/receptionists/doctors", async (CreateDoctorProfileCommand command, CreateDoctorProfileHandler handler) =>
    {
        await handler.Handle(command);
        return Results.Ok();
    });

    app.Run();
}

public partial class Program { }