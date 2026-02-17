using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using InnoClinic.Shared;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Services;

internal sealed class CreateService(
    AppDbContext context,
    IValidator<CreateService.Request> validator)
{
    public sealed record Request(
        string ServiceName,
        decimal Price,
        Guid CategoryId,
        Guid SpecializationId,
        bool IsActive);

    public async Task<Guid> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var service = new Service
        {
            Id = Guid.NewGuid(),
            ServiceName = request.ServiceName,
            Price = request.Price,
            CategoryId = request.CategoryId,
            SpecializationId = request.SpecializationId,
            IsActive = request.IsActive
        };

        context.Services.Add(service);
        await context.SaveChangesAsync();

        return service.Id;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("services", async (Request request, CreateService useCase) =>
            {
                Guid serviceId = await useCase.Handle(request);
                return Results.Created($"/services/{serviceId}", serviceId);
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequireAuthorization(policy => policy.RequireRole(Roles.Receptionist));
        }
    }
}