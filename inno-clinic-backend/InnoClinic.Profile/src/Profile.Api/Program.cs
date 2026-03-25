using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Extensions;
using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
using Profile.Features.Doctors.EditDoctorStatus;
using Profile.Features.Doctors.EditDoctorStatusByBot;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using Profile.Infrastructure;
using Profile.Infrastructure.Database;
using Profile.Infrastructure.Services;

using Quartz;

using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;
var host = builder.Host;

services.AddEndpointsApiExplorer();
services.AddHttpClient();

builder.AddMongoDBClient("notifications-db");

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("StatusJob");

    q.AddJob<EmailReminderJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("TelegramStatusJob-trigger")
        // .WithCronSchedule("0 * * ? * *")
        .WithCronSchedule("0 0 16 ? * MON-FRI")
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

host.UseWolverine(opts =>
    {
        var rabbitConnectionString = builder.Configuration.GetConnectionString("rabbitmq");

        if (rabbitConnectionString != null)
        {
            opts.UseRabbitMq(new Uri(rabbitConnectionString))
                .AutoProvision();
        }
        opts.PublishMessage<DoctorCreated>()
            .ToRabbitExchange("doctor-created-events");

        opts.PublishMessage<SendDailyPollCommand>()
            .ToRabbitExchange("notifications");

        opts.PublishMessage<TelegramAccountLinked>()
            .ToRabbitExchange("telegram-account-linked-events");

        opts.Discovery.IncludeAssembly(typeof(CreateDoctorProfileCommandHandler).Assembly);
        opts.Discovery.IncludeAssembly(typeof(EmailReminderJob).Assembly);
        opts.Discovery.IncludeAssembly(typeof(LinkTelegramAccountCommandHandler).Assembly);
    }
);

services.AddHealthChecks();

services
        .AddWebHostInfrastructure(configuration, environment)
        .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.InitializeAsync();

app.MapPost("/receptionists/doctors", async (CreateDoctorProfileCommand command, IMessageBus bus) =>
    await bus.InvokeAsync(command))
        .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapPut("/doctors/status", async (EditDoctorStatusCommand command, IMessageBus bus) =>
    await bus.InvokeAsync(command))
        .RequireAuthorization(policy => policy.RequireRole(Roles.Doctor));

app.MapPut("/bot/doctors/status", async (EditDoctorStatusByBotCommand command, IMessageBus bus) =>
    await bus.InvokeAsync(command))
        .RequireAuthorization("BotPolicy");

app.MapPost("/bot/accounts/link-telegram", async (LinkTelegramAccountCommand command, IMessageBus bus) =>
    await bus.InvokeAsync(command))
        .RequireAuthorization("BotPolicy");

app.MapGet("/{accountId:guid}", async (
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
app.MapGet("/doctors/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
    {
        var doctor = await dbContext.Set<Doctor>().FindAsync(id);
        if (doctor is null) return Results.NotFound();
        return Results.Ok(new DoctorDto(doctor.Id, doctor.FirstName.Value, doctor.LastName.Value, doctor.MiddleName.Value, doctor.Status.Value == Statuses.AtWork));
    });

app.MapGet("/patients/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
    {
        var patient = await dbContext.Set<Patient>().FindAsync(id);
        if (patient is null) return Results.NotFound();
        return Results.Ok(new PatientDto(patient.Id, patient.FirstName.Value, patient.LastName.Value, patient.MiddleName.Value));
    });

app.MapGet("/patients/{id:guid}/is-linked", async (Guid id, ProfileDbContext dbContext) =>
    {
        var patient = await dbContext.Set<Patient>().FindAsync(id);
        if (patient is null || patient.IsLinkedToAccount) return Results.NotFound();
        return Results.Ok(true);
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

await app.RunAsync();

public partial class Program { }