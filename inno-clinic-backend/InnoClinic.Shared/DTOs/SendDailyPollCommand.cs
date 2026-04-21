namespace InnoClinic.Shared.DTOs;

public record SendDailyPollCommand(
    Guid AccountId,
    DateTime PollDate,
    string Message,
    string TelegramLink
);