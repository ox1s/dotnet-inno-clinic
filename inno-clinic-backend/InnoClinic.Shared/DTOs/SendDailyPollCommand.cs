namespace InnoClinic.Shared.DTOs;

public record SendDailyPollCommand(
    Guid AccountId,
    string Email,
    DateTime PollDate
);