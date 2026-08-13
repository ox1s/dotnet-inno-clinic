using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Specializations
{
    public class DeleteSpecialization(AppDbContext context)
    {
        public async Task<bool> Handle(Guid id)
        {
            Specialization? specialization = await context.Specializations.FindAsync(id);
            if (specialization is null)
                return false;

            var isThereServiceForThisSpecialization = await context.Services.AnyAsync(s => s.SpecializationId == id);
            if (isThereServiceForThisSpecialization)
                throw new ConflictException("There is services for this specialization, so you don't have opportunity to delete it");

            context.Specializations.Remove(specialization);
            await context.SaveChangesAsync();

            return true;
        }
        internal sealed class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapDelete("specializations/{id:guid}", async (Guid id, DeleteSpecialization useCase) =>
                {
                    var deleted = await useCase.Handle(id);
                    return deleted ? Results.NoContent() : Results.NotFound();
                })
                .WithTags(SpecializationEndpoints.Tag)
                .RequirePermission(Permissions.SpecializationsManipulate);
            }
        }

    }
}
