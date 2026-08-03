using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class ChangeOfficeStatus(
    AppDbContext context,
    IValidator<ChangeOfficeStatus.Request> validator)
{
    public sealed record Request(bool IsActive);

    public async Task<bool> Handle(Guid id, Request request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var office = await context.Offices
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (office is null) return false;

        office.IsActive = request.IsActive;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.IsActive).NotNull();
        }
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("offices/{id:guid}/status", async (Guid id, Request request, ChangeOfficeStatus useCase, CancellationToken ct) =>
            {
                var updated = await useCase.Handle(id, request, ct);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate);
        }
    }
}

