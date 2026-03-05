using Profile.Domain.Entities.AccountProfiles;

namespace Profile.Domain.Entities.Doctors;

public class Doctor : AccountProfile
{
    public DateOnly DateOfBirth { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public CareerStartYear CareerStartYear { get; set; }
    public string Status { get; set; } = null!;


    private Doctor(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        DateOnly dateOfBirth,
        Guid accountId,
        Guid specializationId,
        Guid officeId,
        CareerStartYear careerStartYear,
        string status
        ) : base(firstName,
                 lastName,
                 middleName,
                 accountId)
    {
        DateOfBirth = dateOfBirth;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
        Status = status;
    }

    public static Doctor Create(
        FirstName firstName,
        LastName lastName,
        MiddleName middleName,
        DateOnly dateOfBirth,
        Guid accountId,
        Guid specializationId,
        Guid officeId,
        CareerStartYear careerStartYear,
        string status)
    {
        return new Doctor(
            firstName,
            lastName,
            middleName,
            dateOfBirth,
            accountId,
            specializationId,
            officeId,
            careerStartYear,
            status);
    }
}