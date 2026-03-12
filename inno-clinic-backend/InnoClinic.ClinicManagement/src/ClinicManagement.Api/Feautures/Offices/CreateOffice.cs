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

    public async Task<Guid> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var photo = new Photo(request.PhotoUrl) ?? throw new ApplicationException("The photo is invalid");

        var office = Office.Create(
            request.Address,
            photo,
            request.RegistryPhoneNumber,
            request.IsActive);

        context.Offices.Add(office);
        await context.SaveChangesAsync();

        return office.Id;
    }
    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("offices", async (Request request, CreateOffice useCase) =>
            {
                Guid officeId = await useCase.Handle(request);
                return Results.Created($"/offices/{officeId}", officeId);
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate);
        }
    }
}