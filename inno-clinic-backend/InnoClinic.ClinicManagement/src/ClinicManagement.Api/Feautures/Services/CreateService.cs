using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;

using Microsoft.EntityFrameworkCore;

using FluentValidation;

namespace ClinicManagement.Api.Features.Services;

internal sealed class CreateService(
    AppDbContext context,
    IValidator<CreateService.Request> validator)
{
    public sealed record Request(
        string ServiceName,
        decimal Price,
        string Currency,
        Guid CategoryId,
        bool IsActive,
        Guid SpecializationId);

    public sealed record Response(
        Guid Id,
        string Name,
        decimal Price,
        string Currency,
        Guid CategoryId,
        Guid SpecializationId
    );

    public async Task<Response> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var currency = Currency.FromCode(request.Currency);
        var price = new Price(request.Price, currency);

        if (price is null || currency is null)
            throw new ValidationException("The price is invalid");

        var isCategoryExist = await context.ServiceCategories.AnyAsync(c => c.Id == request.CategoryId);
        if (!isCategoryExist)
            throw new ValidationException("The category is invalid");

        var isSpecializationExist = await context.Specializations.AnyAsync(s => s.Id == request.SpecializationId);
        if (!isSpecializationExist)
            throw new ValidationException("The specialization is invalid");

        var service = Service.Create(
            request.CategoryId,
            request.ServiceName,
            price,
            request.IsActive,
            request.SpecializationId
            );

        context.Services.Add(service);
        await context.SaveChangesAsync();

        return new Response(
            service.Id,
            service.ServiceName,
            service.Price.Amount,
            service.Price.Currency.Code,
            service.CategoryId,
            service.SpecializationId
        );
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("services", async (Request request, CreateService useCase) =>
            {
                var response = await useCase.Handle(request);
                return Results.Created(
                    $"/services/{response.Id}",
                    response);
            })
            .WithTags(ServiceEndpoints.Tag)
            .RequirePermission(Permissions.SpecializationsManipulate);
        }
    }
}