using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

namespace ClinicManagement.Api.Offices;

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
}