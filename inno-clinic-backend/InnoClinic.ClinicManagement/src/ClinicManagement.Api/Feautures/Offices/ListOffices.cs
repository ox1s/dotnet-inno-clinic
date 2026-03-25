using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Features.Offices;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Offices;

internal sealed class ListOffices(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string Address,
        string RegistryPhoneNumber,
        bool IsActive);

    public async Task<IReadOnlyList<Response>> Handle(bool? isActive, CancellationToken cancellationToken)
    {
        var query = context.Offices.AsNoTracking();

        if (isActive is not null)
        {
            query = query.Where(o => o.IsActive == isActive);
        }

        return await query
            .OrderBy(o => o.Address)
            .Select(o => new Response(o.Id, o.Address, o.RegistryPhoneNumber, o.IsActive))
            .ToListAsync(cancellationToken);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("offices", async (bool? isActive, ListOffices useCase, CancellationToken ct) =>
                Results.Ok(await useCase.Handle(isActive, ct)))
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesRead);
        }
    }
}

