namespace InnoClinic.Shared.DTOs;

public record DoctorDto(
    Guid Id,
    string FirstName,
    string LastName,
    string MiddleName,
    bool IsActive
);
