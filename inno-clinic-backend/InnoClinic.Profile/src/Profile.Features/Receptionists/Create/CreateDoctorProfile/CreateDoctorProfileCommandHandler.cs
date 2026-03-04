using Profile.Domain.Entities.AccountProfiles;
using Profile.Domain.Entities.Doctors;
using Profile.Infrastructure.Database;

namespace Profile.Features.Receptionists.Create.CreateDoctorProfile;

public class CreateDoctorProfileCommandHandler(ProfileDbContext dbContext)
{
    public async Task Handle(CreateDoctorProfileCommand command)
    {
        var firstName = FirstName.Create(command.FirstName);
        var lastName = LastName.Create(command.LastName);
        var middleName = MiddleName.Create(command.MiddleName);

        var careerStartYear = CareerStartYear.From(command.CareerStartYear);

        var doctor = Doctor.Create(
            accountId: command.AccountId,
            firstName: firstName,
            lastName: lastName,
            middleName: middleName,
            dateOfBirth: command.DateOfBirth,
            specializationId: command.SpecializationId,
            officeId: command.OfficeId,
            careerStartYear: careerStartYear,
            status: command.Status
        );

        dbContext.Set<Doctor>().Add(doctor);
        await dbContext.SaveChangesAsync();
    }
}
