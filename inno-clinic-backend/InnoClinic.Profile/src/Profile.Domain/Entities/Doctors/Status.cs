using InnoClinic.Shared;

namespace Profile.Domain.Entities.Doctors;

public record Status(string Value)
{
    public static Status From(string value)
    {
        return value switch
        {
            // TODO: Убрать хардкод
            Statuses.AtWork => new Status("At work"),
            Statuses.OnVacation => new Status("On vacation"),
            Statuses.SickDay => new Status("Sick Day"),
            Statuses.SickLeave => new Status("Sick Leave"),
            Statuses.SelfIsolation => new Status("Self-Isolation"),
            Statuses.LeaveWithoutPay => new Status("Leave without pay"),
            _ => throw new ArgumentException($"Invalid status value: {value}"),
        };

    }
}