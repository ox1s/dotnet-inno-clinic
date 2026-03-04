namespace Profile.Features.Receptionists.Create.CreateDoctorProfile;

public record CreateDoctorProfileCommand(
    Guid AccountId,
    string FirstName,
    string LastName,
    string MiddleName,
    DateOnly DateOfBirth,
    string Email,
    Guid SpecializationId,
    Guid OfficeId,
    int CareerStartYear,
    string Status
);