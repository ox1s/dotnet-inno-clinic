namespace InnoClinic.Shared.DTOs;

public record ServiceDto(
    Guid Id,
    string Name,
    bool IsActive
);