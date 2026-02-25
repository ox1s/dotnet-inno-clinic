using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Offices;

internal sealed class DeleteOffice(AppDbContext context)
{
    public async Task<bool> Handle(Guid officeId)
    {
        Office? office = await context.Offices.FindAsync(officeId);

        if (office is null) return false;

        context.Offices.Remove(office);

        await context.SaveChangesAsync();

        return true;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("offices/{id:guid}", async (Guid id, DeleteOffice useCase) =>
            {
                bool success = await useCase.Handle(id);
                return success ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequireAuthorization();
        }
    }
}