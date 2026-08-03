using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Services;

internal sealed class ChangeServiceStatus(
    AppDbContext context,
    IValidator<ChangeServiceStatus.Request> validator)
{
    public sealed record Request(bool IsActive);

    public async Task<bool> Handle(Guid id, Request request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var service = await context.Services
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (service is null) return false;

        service.ChangeStatus(request.IsActive);
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
            app.MapPatch("services/{id:guid}/status", async (Guid id, Request request, ChangeServiceStatus useCase, CancellationToken ct) =>
            {
                var updated = await useCase.Handle(id, request, ct);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}

