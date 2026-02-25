namespace InnoClinic.Shared.DTOs;

public record CheckStatus(
    Guid EntityId
);
// for services and offices "Active"
// for doctors "At work"