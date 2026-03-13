using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Services;

internal sealed class ListServices(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string ServiceName,
        decimal Price,
        string Currency,
        CategoryDto Category,
        bool IsActive,
        Guid? SpecializationId);

    public async Task<IEnumerable<Response>> Handle(bool? isActive, Guid? specializationId)
    {
        var query = context.Services.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        if (specializationId.HasValue)
        {
            query = query.Where(s => s.SpecializationId == specializationId.Value);
        }

        return await query
            .Select(s => new Response(
                s.Id,
                s.ServiceName,
                s.Price.Amount,
                s.Price.Currency.Code,
                new CategoryDto(
                    s.Category.Id,
                    s.Category.Name
                ),
                s.IsActive,
                s.SpecializationId))
            .ToListAsync();
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("services", async (bool? isActive, Guid? specializationId, ListServices useCase) =>
            {
                var response = await useCase.Handle(isActive, specializationId);
                return Results.Ok(response);
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsRead);
        }
    }
}