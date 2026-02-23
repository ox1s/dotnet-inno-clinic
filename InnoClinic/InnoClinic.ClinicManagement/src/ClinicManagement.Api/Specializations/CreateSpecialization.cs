using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Services;

using FluentValidation;
namespace ClinicManagement.Api.Specializations;

public class CreateSpecialization(
    AppDbContext context)
{
    public sealed record Request(
        string SpecializationName,
        bool IsActive);

    public async Task<Guid> Handle(Request request)
    {

        var specialization = Specialization.Create(
            request.SpecializationName,
            request.IsActive);
        context.Specializations.Add(specialization);
        await context.SaveChangesAsync();

        return specialization.Id;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("specializations", async (Request request, CreateSpecialization useCase) =>
                {
                    Guid specializationId = await useCase.Handle(request);
                    return Results.Created($"/specializations/{specializationId}", specializationId);
                })
                .WithTags(SpecializationEndpoints.Tag)
                .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}