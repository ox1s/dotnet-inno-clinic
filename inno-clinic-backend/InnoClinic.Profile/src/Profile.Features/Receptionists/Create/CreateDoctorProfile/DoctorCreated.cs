namespace Profile.Features.Receptionists.Create.CreateDoctorProfile;

public record DoctorCreated(
    string Email,
    Guid AccountId
);
