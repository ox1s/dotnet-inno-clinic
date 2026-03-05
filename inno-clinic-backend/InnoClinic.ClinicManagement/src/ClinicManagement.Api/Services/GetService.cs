using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Services;

internal sealed class GetService(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string ServiceName,
        decimal Price,
        string Currency,
        Guid CategoryId,
        string CategoryName,
        bool IsActive);

    public async Task<Response?> Handle(Guid id)
    {
        var service = await context.Services
            .AsNoTracking()
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
                s.IsActive))
            .FirstOrDefaultAsync(s => s.Id == id);

        return service;
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
            .RequireAuthorization();
        }
    }
}