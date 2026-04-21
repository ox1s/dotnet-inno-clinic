namespace InnoClinic.Shared.DTOs;

public record ReceptionistDto(
    Guid Id,
    string FirstName,
    string LastName,
    string MiddleName,
    Guid OfficeId
);

