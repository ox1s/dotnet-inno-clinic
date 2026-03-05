using Wolverine;
using Wolverine.RabbitMQ;

using Profile.Infrastructure;
using Profile.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Profile.Api.Extensions;
using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using InnoClinic.Shared;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    var configuration = builder.Configuration;
    var environment = builder.Environment;
    var logging = builder.Logging;
    var host = builder.Host;

    // builder.Services.AddQuartz(q =>
    // {
    //     var jobKey = new JobKey("StatusJob");

    //     q.AddJob<SendActivityCheckJob>(opts => opts.WithIdentity(jobKey));

    //     q.AddTrigger(opts => opts
    //         .ForJob(jobKey)
    //         .WithIdentity("TelegramStatusJob-trigger")
    //         .WithCronSchedule("0 0 14 ? * MON-FRI")
    //     );
    // });

    // builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    services.AddEndpointsApiExplorer();
    services.AddHttpClient();

    host.UseWolverine(opts =>
    {
        opts.UseRabbitMqUsingNamedConnection("rabbitmq")
            .AutoProvision()
            .UseConventionalRouting();

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

        if (receptionistExists)
        {
            return Results.Ok(new { Role = Roles.Receptionist, Status = "Active" });
        }

        var doctor = await dbContext.Set<Doctor>()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);

        if (doctor is not null)
        {
            return Results.Ok(new { Role = Roles.Doctor, Status = doctor.Status });
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



    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultEndpoints();


    app.Run();
}

public partial class Program { }
