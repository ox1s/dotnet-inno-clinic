using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Services;

internal sealed partial class GetService(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string ServiceName,
        decimal Price,
        string Currency,
        CategoryDTO Category,
        bool IsActive);

    public async Task<Response?> Handle(Guid id)
    {
        return await context.Services
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new Response(
                s.Id,
                s.ServiceName,
                s.Price.Amount,
                s.Price.Currency.Code,
                new CategoryDTO(
                    s.Category.Id,
                    s.Category.Name),
                s.IsActive))
            .FirstOrDefaultAsync();
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("services/{id:guid}", async (Guid id, GetService useCase) =>
            {
                var response = await useCase.Handle(id);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsRead);
        }
    }
}