using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Specializations
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
                app.MapDelete("specializations/{guid:id}", async (Guid id, DeleteSpecialization useCase) =>
                {
                    var response = await useCase.Handle(id);
                    return response ? Results.Ok(response)
                    : Results.Conflict();
                });
            }
        }

    }
}
