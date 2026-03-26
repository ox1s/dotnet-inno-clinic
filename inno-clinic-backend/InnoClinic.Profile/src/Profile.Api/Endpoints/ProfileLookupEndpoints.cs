using InnoClinic.Shared;
using InnoClinic.Shared.DTOs;

using Microsoft.EntityFrameworkCore;

using Profile.Api.Common;

using Profile.Domain.Entities.Doctors;
using Profile.Domain.Entities.Patients;
using Profile.Domain.Entities.Receptionists;
using Profile.Infrastructure.Database;

namespace Profile.Api.Endpoints;

public sealed class ProfileLookupEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
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
    }
}
