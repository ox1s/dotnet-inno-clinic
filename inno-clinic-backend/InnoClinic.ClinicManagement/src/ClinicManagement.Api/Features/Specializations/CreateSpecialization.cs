using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Specializations;

public class CreateSpecialization(
    AppDbContext context,
    IValidator<CreateSpecialization.Request> validator)
{
    public sealed record Request(
        string SpecializationName,
        bool IsActive);
    public sealed record Response(
        Guid Id,
        string Name);

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var existingSpecialization = await context.Specializations
            .FirstOrDefaultAsync(s =>
                s.SpecializationName == request.SpecializationName, cancellationToken: cancellationToken);

        if (existingSpecialization is not null)
            throw new ConflictException("Specialization already exists");

        var specialization = Specialization.Create(
            request.SpecializationName,
            request.IsActive);

        context.Specializations.Add(specialization);
        await context.SaveChangesAsync(cancellationToken);

        return new Response(specialization.Id, specialization.SpecializationName);
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.SpecializationName).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("specializations", async (Request request, CreateSpecialization useCase, CancellationToken cts) =>
                {
                    var response = await useCase.Handle(request, cts);
                    return Results.Ok(new Response(response.Id, request.SpecializationName));
                })
                .WithTags(SpecializationEndpoints.Tag)
                .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}