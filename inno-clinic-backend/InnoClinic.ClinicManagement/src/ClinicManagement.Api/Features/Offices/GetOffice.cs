using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Offices;

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
        var office = await context.Offices
            .AsNoTracking()
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (office is null)
        {
            return null;
        }

        return new Response(
            office.Id,
            office.Address,
            office.Photo.Url,
            office.RegistryPhoneNumber,
            office.IsActive);
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

