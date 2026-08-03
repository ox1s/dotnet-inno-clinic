using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Specializations;

internal sealed class UpdateSpecialization(
    AppDbContext context,
    IValidator<UpdateSpecialization.Request> validator)
{
    public sealed record Request(string Name, bool IsActive);

    public async Task<bool> Handle(Guid id, Request request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var specialization = await context.Specializations
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (specialization is null) return false;

        specialization.Update(request.Name, request.IsActive);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("specializations/{id:guid}", async (Guid id, Request request, UpdateSpecialization useCase, CancellationToken ct) =>
            {
                var updated = await useCase.Handle(id, request, ct);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(SpecializationEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}

