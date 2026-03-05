using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Services;

internal sealed class ListServices(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string ServiceName,
        decimal Price,
        string Currency,
        Guid CategoryId,
        string CategoryName,
        Guid SpecializationId,
        bool IsActive);

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
                s.CategoryId,
                context.ServiceCategories
                    .Where(c => c.Id == s.CategoryId)
                    .Select(c => c.Name)
                    .FirstOrDefault() ?? string.Empty,
                s.SpecializationId,
                s.IsActive))
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
            .RequireAuthorization();
        }
    }
}