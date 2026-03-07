namespace InnoClinic.Shared.DTOs;

public record LinkTelegramAccountCommand(Guid AccountId, string TelegramId);