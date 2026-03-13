using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Categories;

internal sealed partial class ListCategories(AppDbContext dbContext)
{
    public sealed record Response(Guid Id, string Name);

    public async Task<List<Response>> Handle(Guid id)
    {
        return await dbContext.ServiceCategories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new Response(c.Id, c.Name))
            .ToListAsync();
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories/", async (Guid id, ListCategories useCase) =>
            {
                var response = await useCase.Handle(id);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(CategoryEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsRead);
        }
    }
}
