using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Contracts;
using Profile.Api.Extensions;
using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
using Profile.Domain.Entities.AccountProfiles;
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
    var receptionist = await dbContext.Set<Receptionist>()
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.AccountId == accountId, cancellationToken);

    if (receptionist is not null)
    {
        return Results.Ok(new ProfileDataDto(
            Role: Roles.Receptionist,
            Status: receptionist.IsDeleted ? "Inactive" : "Active"));
    }

    var doctor = await dbContext.Set<Doctor>()
        .AsNoTracking()
        .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);

    if (doctor is not null)
    {
        return Results.Ok(new ProfileDataDto(
            Role: Roles.Doctor,
            Status: doctor.IsDeleted ? "Inactive" : doctor.Status.Value));
    }

    var patient = await dbContext.Set<Patient>()
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken);

    if (patient is not null)
    {
        return Results.Ok(new ProfileDataDto(
            Role: Roles.Patient,
            Status: patient.IsDeleted ? "Inactive" : "Active"));
    }

    return Results.NotFound();
});
app.MapGet("/doctors/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
    {
        var doctor = await dbContext.Set<Doctor>().FindAsync(id);
        if (doctor is null || doctor.IsDeleted) return Results.NotFound();
        return Results.Ok(new DoctorDto(doctor.Id, doctor.FirstName.Value, doctor.LastName.Value, doctor.MiddleName.Value, doctor.Status.Value == Statuses.AtWork));
    });

app.MapGet("/patients/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
    {
        var patient = await dbContext.Set<Patient>().FindAsync(id);
        if (patient is null || patient.IsDeleted) return Results.NotFound();
        return Results.Ok(new PatientDto(patient.Id, patient.FirstName.Value, patient.LastName.Value, patient.MiddleName.Value));
    });

// Appointment service uses account id from JWT claim as "patient id". Keep this route, but interpret {id} as AccountId.
app.MapGet("/patients/{id:guid}/is-linked", async (Guid id, ProfileDbContext dbContext) =>
    {
        var patient = await dbContext.Set<Patient>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.AccountId == id && !p.IsDeleted);

        if (patient is null) return Results.NotFound();
        return Results.Ok(patient.IsLinkedToAccount);
    });

app.MapGet("/accounts/{accountId:guid}/patient/is-linked", async (Guid accountId, ProfileDbContext dbContext) =>
    {
        var patient = await dbContext.Set<Patient>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.AccountId == accountId && !p.IsDeleted);

        if (patient is null) return Results.NotFound();
        return Results.Ok(patient.IsLinkedToAccount);
    });

app.MapGet("/doctors", async (string? q, int? page, int? pageSize, ProfileDbContext dbContext, CancellationToken ct) =>
    {
        var (normalizedPage, normalizedPageSize) =
            Paging.Normalize(page ?? Paging.DefaultPage, pageSize ?? Paging.DefaultPageSize);

        var query = dbContext.Set<Doctor>()
            .AsNoTracking()
            .Where(d => !d.IsDeleted);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var trimmed = q.Trim();
            query = query.Where(d =>
                (d.FirstName.Value + " " + d.LastName.Value + " " + d.MiddleName.Value).Contains(trimmed));
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(d => d.LastName.Value)
            .ThenBy(d => d.FirstName.Value)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(d => new DoctorDto(d.Id, d.FirstName.Value, d.LastName.Value, d.MiddleName.Value, d.Status.Value == Statuses.AtWork))
            .ToListAsync(ct);

        return Results.Ok(new PagedResponse<DoctorDto>(Items: items, Page: normalizedPage, PageSize: normalizedPageSize, TotalCount: totalCount));
    });

app.MapGet("/patients", async (string? q, int? page, int? pageSize, ProfileDbContext dbContext, CancellationToken ct) =>
    {
        var (normalizedPage, normalizedPageSize) =
            Paging.Normalize(page ?? Paging.DefaultPage, pageSize ?? Paging.DefaultPageSize);

        var query = dbContext.Set<Patient>()
            .AsNoTracking()
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var trimmed = q.Trim();
            query = query.Where(p =>
                (p.FirstName.Value + " " + p.LastName.Value + " " + p.MiddleName.Value).Contains(trimmed));
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.LastName.Value)
            .ThenBy(p => p.FirstName.Value)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(p => new PatientDto(p.Id, p.FirstName.Value, p.LastName.Value, p.MiddleName.Value))
            .ToListAsync(ct);

        return Results.Ok(new PagedResponse<PatientDto>(Items: items, Page: normalizedPage, PageSize: normalizedPageSize, TotalCount: totalCount));
    }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapGet("/receptionists", async (string? q, int? page, int? pageSize, ProfileDbContext dbContext, CancellationToken ct) =>
    {
        var (normalizedPage, normalizedPageSize) =
            Paging.Normalize(page ?? Paging.DefaultPage, pageSize ?? Paging.DefaultPageSize);

        var query = dbContext.Set<Receptionist>()
            .AsNoTracking()
            .Where(r => !r.IsDeleted);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var trimmed = q.Trim();
            query = query.Where(r =>
                (r.FirstName.Value + " " + r.LastName.Value + " " + r.MiddleName.Value).Contains(trimmed));
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(r => r.LastName.Value)
            .ThenBy(r => r.FirstName.Value)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .Select(r => new ReceptionistDto(
                r.Id,
                r.FirstName.Value,
                r.LastName.Value,
                r.MiddleName.Value,
                r.OfficeId))
            .ToListAsync(ct);

        return Results.Ok(new PagedResponse<ReceptionistDto>(Items: items, Page: normalizedPage, PageSize: normalizedPageSize, TotalCount: totalCount));
    }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapPost("/receptionists/patients", async (
    CreatePatientProfileRequest request,
    ProfileDbContext dbContext,
    CancellationToken ct) =>
{
    var patientExists = await dbContext.Set<Patient>()
        .AsNoTracking()
        .AnyAsync(p => p.AccountId == request.AccountId && !p.IsDeleted, ct);

    if (patientExists)
    {
        return Results.Conflict("Patient profile already exists for this account.");
    }

    var patient = Patient.Create(
        FirstName.Create(request.FirstName),
        LastName.Create(request.LastName),
        MiddleName.Create(request.MiddleName ?? string.Empty),
        request.IsLinkedToAccount,
        DateOnly.FromDateTime(request.DateOfBirth),
        request.AccountId);

    dbContext.Set<Patient>().Add(patient);
    await dbContext.CommitChangesAsync(ct);

    return Results.Ok(new PatientDto(patient.Id, patient.FirstName.Value, patient.LastName.Value, patient.MiddleName.Value));
}).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapPost("/receptionists/receptionists", async (
    CreateReceptionistProfileRequest request,
    ProfileDbContext dbContext,
    CancellationToken ct) =>
{
    var receptionistExists = await dbContext.Set<Receptionist>()
        .AsNoTracking()
        .AnyAsync(r => r.AccountId == request.AccountId && !r.IsDeleted, ct);

    if (receptionistExists)
    {
        return Results.Conflict("Receptionist profile already exists for this account.");
    }

    var receptionist = Receptionist.Create(
        FirstName.Create(request.FirstName),
        LastName.Create(request.LastName),
        MiddleName.Create(request.MiddleName ?? string.Empty),
        request.AccountId,
        request.OfficeId);

    dbContext.Set<Receptionist>().Add(receptionist);
    await dbContext.CommitChangesAsync(ct);

    return Results.Ok(new ReceptionistDto(
        receptionist.Id,
        receptionist.FirstName.Value,
        receptionist.LastName.Value,
        receptionist.MiddleName.Value,
        receptionist.OfficeId));
}).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapDelete("/receptionists/receptionists/{id:guid}", async (
    Guid id,
    ProfileDbContext dbContext,
    CancellationToken ct) =>
{
    var receptionist = await dbContext.Set<Receptionist>().FindAsync([id], ct);
    if (receptionist is null || receptionist.IsDeleted) return Results.NotFound();

    dbContext.Set<Receptionist>().Remove(receptionist);
    await dbContext.CommitChangesAsync(ct);
    return Results.NoContent();
}).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.MapDelete("/receptionists/patients/{id:guid}", async (
    Guid id,
    ProfileDbContext dbContext,
    CancellationToken ct) =>
{
    var patient = await dbContext.Set<Patient>().FindAsync([id], ct);
    if (patient is null || patient.IsDeleted) return Results.NotFound();

    dbContext.Set<Patient>().Remove(patient);
    await dbContext.CommitChangesAsync(ct);
    return Results.NoContent();
}).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

await app.RunAsync();

public partial class Program { }
