namespace InnoClinic.Shared.DTOs;

public record SendVerificationEmailCommand(
    Guid AccountId,
    string Email,
    string VerificationLink
);