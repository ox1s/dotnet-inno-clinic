using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Specializations;

internal sealed class ListSpecializations(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string Name,
        bool IsActive);

    public async Task<IReadOnlyList<Response>> Handle(bool? isActive, CancellationToken cancellationToken)
    {
        var query = context.Specializations.AsNoTracking();

        if (isActive is not null)
        {
            query = query.Where(s => s.IsActive == isActive);
        }

        return await query
            .OrderBy(s => s.SpecializationName)
            .Select(s => new Response(s.Id, s.SpecializationName, s.IsActive))
            .ToListAsync(cancellationToken);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("specializations", async (bool? isActive, ListSpecializations useCase, CancellationToken ct) =>
                Results.Ok(await useCase.Handle(isActive, ct)))
            .WithTags(SpecializationEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsRead);
        }
    }
}

