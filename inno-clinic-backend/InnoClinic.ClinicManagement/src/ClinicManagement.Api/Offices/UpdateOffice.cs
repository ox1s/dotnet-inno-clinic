using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

using FluentValidation;

namespace ClinicManagement.Api.Offices;

internal sealed class UpdateOffice(
    AppDbContext context,
    IValidator<UpdateOffice.Request> validator)
{
    public sealed record Request(
        Guid Id,
        string Address,
        string PhotoUrl,
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

        var photo = new Photo(request.PhotoUrl);

        office.Update(
            request.Address,
            request.RegistryPhoneNumber,
            photo,
            request.IsActive);

        await context.SaveChangesAsync();

        return true;
    }
}