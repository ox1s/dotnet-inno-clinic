namespace InnoClinic.Shared.Contracts.Notifications;

public record SendVerificationEmailCommand(
    Guid AccountId,
    string Email,
    string VerificationLink
);