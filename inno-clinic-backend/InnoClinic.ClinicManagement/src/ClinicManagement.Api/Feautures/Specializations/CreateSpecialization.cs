using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Exceptions;

using Microsoft.EntityFrameworkCore;
namespace ClinicManagement.Api.Features.Specializations;

public class CreateSpecialization(
    AppDbContext context)
{
    public sealed record Request(
        string SpecializationName,
        bool IsActive);
    public sealed record Response(
        Guid Id,
        string Name);

    public async Task<Response> Handle(Request request)
    {
        var existingSpecialization = await context.Specializations
            .FirstOrDefaultAsync(s =>
                s.SpecializationName == request.SpecializationName);

        if (existingSpecialization is not null)
            throw new ConflictException("Specialization already exists");

        var isThereAnyServices = await context.Services.AnyAsync();
        if (!isThereAnyServices)
            throw new NotFoundException("Specialization can't be created when there are no services");

        var specialization = Specialization.Create(
            request.SpecializationName,
            request.IsActive);

        context.Specializations.Add(specialization);
        await context.SaveChangesAsync();

        return new Response(specialization.Id, specialization.SpecializationName);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("specializations", async (Request request, CreateSpecialization useCase) =>
                {
                    var response = await useCase.Handle(request);
                    return Results.Ok(new Response(response.Id, request.SpecializationName));
                })
                .WithTags(SpecializationEndpoints.Tag)
                .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}