using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Common;

using Profile.Api.Contracts;
using Profile.Domain.Entities.AccountProfiles;
using Profile.Domain.Entities.Receptionists;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using Profile.Infrastructure.Database;

using Wolverine;

namespace Profile.Api.Endpoints;

public sealed class ReceptionistEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/doctors", async (CreateDoctorProfileCommand command, IMessageBus bus) =>
                await bus.InvokeAsync(command))
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

        app.MapGet("/receptionists", async (
                string? q,
                int? page,
                int? pageSize,
                ProfileDbContext dbContext,
                CancellationToken ct) =>
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

            return Results.Ok(new PagedResponse<ReceptionistDto>(
                Items: items,
                Page: normalizedPage,
                PageSize: normalizedPageSize,
                TotalCount: totalCount));
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

        app.MapPost("/receptionists", async (
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

        app.MapDelete("/receptionists/{id:guid}", async (
                Guid id,
                ProfileDbContext dbContext,
                CancellationToken ct) =>
        {
            var receptionist = await dbContext.Set<Receptionist>().FindAsync([id], ct);
            if (receptionist is null || receptionist.IsDeleted) return Results.NotFound();

            receptionist.IsDeleted = true;
            await dbContext.CommitChangesAsync(ct);
            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
    }
}
