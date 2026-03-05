using InnoClinic.Shared;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Extensions;
using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using Profile.Infrastructure;
using Profile.Infrastructure.Database;

using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    var configuration = builder.Configuration;
    var environment = builder.Environment;
    var logging = builder.Logging;
    var host = builder.Host;

    services.AddEndpointsApiExplorer();
    services.AddHttpClient();

    host.UseWolverine(opts =>
        {
            opts.PublishMessage<DoctorCreated>().ToRabbitExchange("telegram-notifications-queue");

            var rabbitConnectionString = builder.Configuration.GetConnectionString("rabbitmq");

            if (rabbitConnectionString != null)
            {
                opts.UseRabbitMq(new Uri(rabbitConnectionString))
                    .AutoProvision()
                    .UseConventionalRouting();
            }

            opts.Discovery.IncludeAssembly(typeof(CreateDoctorProfileCommandHandler).Assembly);
        }
    );

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

    app.MapPost("/receptionists/doctors", async (CreateDoctorProfileCommand command, IMessageBus bus) =>
        await bus.InvokeAsync(command))
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

    app.MapGet("/profiles/{accountId:guid}", async (
        Guid accountId,
        ProfileDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        var receptionistExists = await dbContext.Set<Receptionist>()
            .AsNoTracking()
            .AnyAsync(r => r.AccountId == accountId, cancellationToken);

        if (receptionistExists) return Results.Ok(new { Role = Roles.Receptionist, Status = "Active" });

        var doctor = await dbContext.Set<Doctor>()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);

        if (doctor is not null) return Results.Ok(new { Role = Roles.Doctor, doctor.Status });

        var patientExists = await dbContext.Set<Patient>()
            .AsNoTracking()
            .AnyAsync(p => p.AccountId == accountId, cancellationToken);

        if (patientExists) return Results.Ok(new { Role = Roles.Patient, Status = "Active" });

        return Results.NotFound();
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultEndpoints();

    app.Run();
}

public partial class Program { }