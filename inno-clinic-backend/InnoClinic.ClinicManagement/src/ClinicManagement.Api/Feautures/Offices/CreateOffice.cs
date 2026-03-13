using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class CreateOffice(
    AppDbContext context,
    IValidator<CreateOffice.Request> validator)
{
    public sealed record Request(
        string Address,
        string PhotoUrl,
        string RegistryPhoneNumber,
        bool IsActive);
    public sealed record Response(Guid Id);

    public async Task<Response> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var photo = new Photo(request.PhotoUrl) ??
            throw new ValidationException("The photo is invalid");

        var office = Office.Create(
            request.Address,
            photo,
            request.RegistryPhoneNumber,
            request.IsActive);

        context.Offices.Add(office);
        await context.SaveChangesAsync();

        return new Response(office.Id);
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("offices", async (Request request, CreateOffice useCase) =>
            {
                var response = await useCase.Handle(request);
                return Results.Created($"/offices/{response.Id}", response);
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate);
        }
    }
}