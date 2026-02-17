using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Offices;

internal sealed class CreateOffice(
    AppDbContext context,
    IValidator<CreateOffice.Request> validator)
{
    public sealed record Request(
        string Address,
        Guid? PhotoId,
        string RegistryPhoneNumber,
        bool IsActive);

    public async Task<Guid> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);


        var office = Office.Create(
            request.Address,
            request.PhotoId,
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
            .RequireAuthorization();
        }
    }
}