using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Categories;

internal sealed partial class ListCategories(AppDbContext dbContext)
{
    public sealed record Response(Guid Id, string Name);

    public async Task<List<Response>> Handle()
    {
        return await dbContext.ServiceCategories
            .AsNoTracking()
            .Select(c => new Response(c.Id, c.Name))
            .ToListAsync();
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("categories/", async (ListCategories useCase) =>
            {
                var response = await useCase.Handle();
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(CategoryEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsRead);
        }
    }
}
