namespace Profile.Features.Doctors.EditDoctorStatusByBot;

public record EditDoctorStatusByBotCommand(
    Guid AccountId,
    string Status
);