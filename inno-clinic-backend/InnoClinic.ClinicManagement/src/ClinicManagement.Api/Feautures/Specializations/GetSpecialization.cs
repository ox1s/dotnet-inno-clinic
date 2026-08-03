using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Features.Specializations;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Feautures.Specializations;

internal sealed class GetSpecialization(AppDbContext context)
{
    public sealed record Response(
        Guid Id,
        string Name,
        bool IsActive);

    public Task<Response?> Handle(Guid id, CancellationToken cancellationToken)
    {
        return context.Specializations
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new Response(s.Id, s.SpecializationName, s.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("specializations/{id:guid}", async (Guid id, GetSpecialization useCase, CancellationToken ct) =>
            {
                var response = await useCase.Handle(id, ct);
                return response is not null ? Results.Ok(response) : Results.NotFound();
            })
            .WithTags(SpecializationEndpoints.Tag)
            .AllowAnonymous();
        }
    }
}

