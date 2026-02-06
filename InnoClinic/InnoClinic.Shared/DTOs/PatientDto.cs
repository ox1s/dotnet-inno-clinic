namespace InnoClinic.Shared.DTOs;

public record PatientDto(
    Guid Id,
    string FirstName,
    string LastName,
    string MiddleName,
    string PhoneNumber,
    bool IsActive
);