using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using InnoClinic.Shared;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Api.Services;

internal sealed class CreateService(
    AppDbContext context,
    IValidator<CreateService.Request> validator)
{
    public sealed record Request(
        string ServiceName,
        decimal Price,
        string Currency,
        Guid CategoryId,
        Guid SpecializationId,
        bool IsActive);

    public async Task<Guid> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var currency = Currency.FromCode(request.Currency);
        var price = new Price(request.Price, currency);

        if (price is null || currency is null)
        {
            throw new ApplicationException("The price is invalid");
        }

        var service = Service.Create(
            request.CategoryId,
            request.ServiceName,
            price,
            request.SpecializationId,
            request.IsActive);
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
            .RequirePermission(Permissions.ServicesManipulate);
        }
    }
}