using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Services;

internal sealed class GetServiceDurationMinutes(AppDbContext context)
{
    public async Task<int?> Handle(Guid serviceId, CancellationToken cancellationToken)
    {
        var timeSlotSize = await context.Services
            .AsNoTracking()
            .Where(s => s.Id == serviceId)
            .Select(s => (int?)s.Category.TimeSlotSize)
            .FirstOrDefaultAsync(cancellationToken);

        if (timeSlotSize is null) return null;

        // Requirements: slots are 10 minutes; category TimeSlotSize is number of base slots.
        return timeSlotSize.Value * 10;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("services/{id:guid}/duration-minutes", async (Guid id, GetServiceDurationMinutes useCase, CancellationToken ct) =>
            {
                var minutes = await useCase.Handle(id, ct);
                return minutes is not null ? Results.Ok(minutes.Value) : Results.NotFound();
            })
            .WithTags(ServiceEndpoints.Tag);
        }
    }
}

