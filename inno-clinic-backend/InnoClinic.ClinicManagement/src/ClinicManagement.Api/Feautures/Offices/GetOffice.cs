using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Features.Offices;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Offices;

internal sealed class GetOffice(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string Address,
        string PhotoUrl,
        string RegistryPhoneNumber,
        bool IsActive);

    public async Task<Response?> Handle(Guid id, CancellationToken cancellationToken)
    {
        return await context.Offices
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new Response(
                o.Id,
                o.Address,
                o.Photo.Url,
                o.RegistryPhoneNumber,
                o.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("offices/{id:guid}", async (Guid id, GetOffice useCase, CancellationToken ct) =>
            {
                var response = await useCase.Handle(id, ct);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(OfficeEndpoints.Tag)
            .AllowAnonymous();
        }
    }
}

