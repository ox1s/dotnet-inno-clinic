using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class DeleteOffice(AppDbContext context)
{
    public async Task<bool> Handle(Guid officeId)
    {
        Office? office = await context.Offices.FindAsync(officeId);

        if (office is null)
            throw new NotFoundException("Office not found");

        context.Offices.Remove(office);
        await context.SaveChangesAsync();

        return true;
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("offices/{officeId}", async (Guid officeId, DeleteOffice useCase) =>
                await useCase.Handle(officeId)
            )
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate);
        }
    }
}