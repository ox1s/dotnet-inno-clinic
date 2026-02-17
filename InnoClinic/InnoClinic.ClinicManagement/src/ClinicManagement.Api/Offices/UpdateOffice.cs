using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ClinicManagement.Api.Offices;

internal sealed class UpdateOffice(
    AppDbContext context,
    IValidator<UpdateOffice.Request> validator)
{
    public sealed record Request(
        Guid Id,
        string Address,
        Guid? PhotoId,
        string RegistryPhoneNumber,
        bool IsActive);

    public async Task<bool> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        Office? office = await context.Offices.FindAsync(request.Id);

        if (office is null)
        {
            return false;
        }

        office.Update(
            request.Address,
            request.RegistryPhoneNumber,
            request.PhotoId,
            request.IsActive);

        await context.SaveChangesAsync();

        return true;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("offices/{id:guid}", async (Guid id, Request request, UpdateOffice useCase) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Id in the route must match the Id in the request body");

                bool success = await useCase.Handle(request);
                return success ? Results.NoContent() : Results.NotFound();
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequireAuthorization();
        }
    }
}