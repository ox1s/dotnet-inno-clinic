using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Common;

using Profile.Api.Contracts;
using Profile.Domain.Entities.AccountProfiles;
using Profile.Domain.Entities.Patients;
using Profile.Infrastructure.Database;

namespace Profile.Api.Endpoints;

public sealed class PatientEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/patients/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
        {
            var patient = await dbContext.Set<Patient>().FindAsync(id);
            if (patient is null || patient.IsDeleted) return Results.NotFound();

            return Results.Ok(new PatientDto(
                patient.Id,
                patient.FirstName.Value,
                patient.LastName.Value,
                patient.MiddleName.Value));
        }).RequireAuthorization();

        app.MapGet("/patients/{id:guid}/is-linked", async (Guid id, ProfileDbContext dbContext) =>
        {
            var patient = await dbContext.Set<Patient>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.AccountId == id && !p.IsDeleted);

            if (patient is null) return Results.NotFound();
            return Results.Ok(patient.IsLinkedToAccount);
        }).RequireAuthorization();

        app.MapGet("/accounts/{accountId:guid}/patient/is-linked", async (Guid accountId, ProfileDbContext dbContext) =>
        {
            var patient = await dbContext.Set<Patient>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.AccountId == accountId && !p.IsDeleted);

            if (patient is null) return Results.NotFound();
            return Results.Ok(patient.IsLinkedToAccount);
        }).RequireAuthorization();

        app.MapGet("/patients", async (
                string? q,
                int? page,
                int? pageSize,
                ProfileDbContext dbContext,
                CancellationToken ct) =>
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
                .Select(p => new PatientDto(
                    p.Id,
                    p.FirstName.Value,
                    p.LastName.Value,
                    p.MiddleName.Value))
                .ToListAsync(ct);

            return Results.Ok(new PagedResponse<PatientDto>(
                Items: items,
                Page: normalizedPage,
                PageSize: normalizedPageSize,
                TotalCount: totalCount));
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

            return Results.Ok(new PatientDto(
                patient.Id,
                patient.FirstName.Value,
                patient.LastName.Value,
                patient.MiddleName.Value));
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

        app.MapDelete("/receptionists/patients/{id:guid}", async (
                Guid id,
                ProfileDbContext dbContext,
                CancellationToken ct) =>
        {
            var patient = await dbContext.Set<Patient>().FindAsync([id], ct);
            if (patient is null || patient.IsDeleted) return Results.NotFound();

            patient.IsDeleted = true;
            await dbContext.CommitChangesAsync(ct);
            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
    }
}
