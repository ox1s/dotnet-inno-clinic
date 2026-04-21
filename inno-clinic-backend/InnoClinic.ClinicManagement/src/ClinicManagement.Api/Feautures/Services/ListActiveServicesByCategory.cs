using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Features.Services;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Services;

public class ListActiveServicesByCategory(AppDbContext context)
{
    public sealed record Response(
    Guid Id,
    string ServiceName,
    decimal Price,
    string Currency,
    CategoryDto Category,
    SpecializationDto Specialization);

    public async Task<IEnumerable<Response>> Handle(Guid categoryId)
    {
        var query = context.Services.AsNoTracking();

        return await query
            .Where(s => s.Category.Id == categoryId && s.IsActive)
            .Select(s => new Response(
                s.Id,
                s.ServiceName,
                s.Price.Amount,
                s.Price.Currency.Code,
                new CategoryDto(
                    s.Category.Id,
                    s.Category.Name
                ),
                new SpecializationDto(
                    s.Specialization.Id,
                    s.Specialization.SpecializationName
                )
            ))
            .ToListAsync();
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("services/category/{id:guid}", async (Guid id, ListActiveServicesByCategory useCase) =>
            {
                var response = await useCase.Handle(id);
                return Results.Ok(response);
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationByCategoryRead);
        }
    }
}
