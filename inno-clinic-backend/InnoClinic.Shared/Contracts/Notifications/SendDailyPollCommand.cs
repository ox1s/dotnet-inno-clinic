namespace InnoClinic.Shared.Contracts.Notifications;

public record SendDailyPollCommand(
    Guid AccountId,
    string Email,
    DateTime PollDate
);