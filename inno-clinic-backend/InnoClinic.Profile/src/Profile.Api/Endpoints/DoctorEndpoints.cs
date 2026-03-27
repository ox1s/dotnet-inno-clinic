using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Common;

using Profile.Api.Contracts;
using Profile.Domain.Entities.Doctors;
using Profile.Features.Doctors.EditDoctorStatus;
using Profile.Features.Receptionists.Create.CreateDoctorProfile;
using Profile.Infrastructure.Database;

using Wolverine;

namespace Profile.Api.Endpoints;

public sealed class DoctorEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/doctors", async (CreateDoctorProfileCommand command, IMessageBus bus) =>
               await bus.InvokeAsync(command))
           .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));

        app.MapPut("/doctors/status", async (EditDoctorStatusCommand command, IMessageBus bus) =>
                await bus.InvokeAsync(command))
            .RequireAuthorization(policy => policy.RequireRole(Roles.Doctor));

        app.MapGet("/doctors/{id:guid}", async (Guid id, ProfileDbContext dbContext) =>
        {
            var doctor = await dbContext.Set<Doctor>().FindAsync(id);
            if (doctor is null || doctor.IsDeleted) return Results.NotFound();

            return Results.Ok(new DoctorDto(
                doctor.Id,
                doctor.FirstName.Value,
                doctor.LastName.Value,
                doctor.MiddleName.Value,
                doctor.Status.Value == Statuses.AtWork));
        });

        app.MapGet("/doctors", async (
                string? q,
                int? page,
                int? pageSize,
                ProfileDbContext dbContext,
                CancellationToken ct) =>
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
                .Select(d => new DoctorDto(
                    d.Id,
                    d.FirstName.Value,
                    d.LastName.Value,
                    d.MiddleName.Value,
                    d.Status.Value == Statuses.AtWork))
                .ToListAsync(ct);

            return Results.Ok(new PagedResponse<DoctorDto>(
                Items: items,
                Page: normalizedPage,
                PageSize: normalizedPageSize,
                TotalCount: totalCount));
        });
    }
}
