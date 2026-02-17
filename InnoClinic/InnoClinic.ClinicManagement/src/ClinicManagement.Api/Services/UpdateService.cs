using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using InnoClinic.Shared;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Services;

internal sealed class UpdateService(
    AppDbContext context,
    IValidator<UpdateService.Request> validator)
{
    public sealed record Request(
        Guid Id,
        string ServiceName,
        decimal Price,
        Guid CategoryId,
        Guid SpecializationId,
        bool IsActive);

    public async Task<bool> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var service = await context.Services.FindAsync(request.Id);

        if (service is null) return false;

        service.ServiceName = request.ServiceName;
        service.Price = request.Price;
        service.CategoryId = request.CategoryId;
        service.SpecializationId = request.SpecializationId;
        service.IsActive = request.IsActive;

        await context.SaveChangesAsync();

        return true;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("services/{id:guid}", async (Guid id, Request request, UpdateService useCase) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Id in the route must match the Id in the request body");

                bool success = await useCase.Handle(request);
                return success ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
        }
    }
}