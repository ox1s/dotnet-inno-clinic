using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Services;

internal sealed class CheckService(AppDbContext context)
{
    public async Task<bool?> Handle(Guid id)
    {
        var service = await context.Services
                  .AsNoTracking()
                  .Where(s => s.Id == id)
                  .FirstOrDefaultAsync();

        return service?.IsActive;
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("services/{id:guid}/active", async (Guid id, CheckService useCase) =>
            {
                var response = await useCase.Handle(id);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(ServiceEndpoints.Tag);
        }
    }
}
