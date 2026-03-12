using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class DeleteOffice(AppDbContext context)
{
    public async Task<bool> Handle(Guid officeId)
    {
        Office? office = await context.Offices.FindAsync(officeId);

        if (office is null) return false;

        context.Offices.Remove(office);

        await context.SaveChangesAsync();

        return true;
    }
}