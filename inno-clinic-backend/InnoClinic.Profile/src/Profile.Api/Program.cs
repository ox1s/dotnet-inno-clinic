using Wolverine;

using Profile.Infrastructure;
using Profile.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Profile.Api.Extensions;
using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
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

    await app.InitializeAsync();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultEndpoints();

    // Тут ошибка надо попровить Wolverine handler, он не видит его, хотя в проекте он есть и там все нормально, надо разобраться почему так происходит
    app.MapPost("/receptionists/doctors", async (CreateDoctorProfileCommand command, IMessageBus bus) =>
        bus.InvokeAsync(command));

    app.MapGet("/profiles/{accountId:guid}", async (
        Guid accountId,
        ProfileDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        var doctor = await dbContext.Set<Doctor>()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);

        if (doctor is not null)
        {
            return Results.Ok(new { Role = "Doctor", Status = doctor.Status });
        }

        var receptionistExists = await dbContext.Set<Receptionist>()
            .AsNoTracking()
            .AnyAsync(r => r.AccountId == accountId, cancellationToken);

        if (receptionistExists)
        {
            return Results.Ok(new { Role = "Receptionist", Status = "Active" });
        }

        var patientExists = await dbContext.Set<Patient>()
            .AsNoTracking()
            .AnyAsync(p => p.AccountId == accountId, cancellationToken);

        if (patientExists)
        {
            return Results.Ok(new { Role = "Patient", Status = "Active" });
        }

        return Results.NotFound();
    });


    app.Run();
}

public partial class Program { }
