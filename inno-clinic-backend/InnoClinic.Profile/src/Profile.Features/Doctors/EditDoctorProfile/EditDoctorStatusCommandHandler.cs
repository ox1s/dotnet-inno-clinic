using Microsoft.AspNetCore.Http;

using Profile.Domain.Entities.Doctors;
using Profile.Infrastructure.Database.Repositories;
using Profile.Infrastructure.Database;

namespace Profile.Features.Doctors.EditDoctorProfile;

public class EditDoctorStatusCommandHandler
{
    public static async Task Handle(
        HttpContext httpContext,
        EditDoctorStatusCommand command,
        ProfileDbContext dbContext,
        DoctorRepository doctorRepository)
    {
        Guid userId = Guid
            .TryParse(httpContext.User.Claims.Single(claim => claim.Type == "id").Value, out var parsedUserId) ?
            parsedUserId :
            throw new ArgumentException("User context is unavailable");

        var doctor = await doctorRepository
            .GetByIdAsync(userId);
        if (doctor is null) throw new ArgumentException("Doctor not found");

        doctor.Status = Status.From(command.Status);

        await dbContext.SaveChangesAsync();
    }
}
