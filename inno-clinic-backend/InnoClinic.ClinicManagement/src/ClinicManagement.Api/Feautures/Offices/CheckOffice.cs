using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Features.Offices;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Offices;

internal sealed class CheckOffice(AppDbContext context)
{
    public async Task<bool?> Handle(Guid id)
    {
        var office = await context.Offices
            .AsNoTracking()
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();

        return office?.IsActive;
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("offices/{id:guid}/active", async (Guid id, CheckOffice useCase) =>
            {
                var response = await useCase.Handle(id);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(OfficeEndpoints.Tag);
        }
    }
}
