using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;

namespace ClinicManagement.Api.Offices;

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