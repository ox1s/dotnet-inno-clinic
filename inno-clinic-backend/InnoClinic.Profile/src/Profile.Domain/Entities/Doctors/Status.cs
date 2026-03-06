using InnoClinic.Shared;

namespace Profile.Domain.Entities.Doctors;

public record Status(string Value)
{
    public static Status From(string value)
    {
        switch (value)
        {
            case Statuses.AtWork:
                return new Status("At Work");
            case Statuses.OnVacation:
                return new Status("On Vacation");
            case Statuses.SickDay:
                return new Status("Sick Day");
            case Statuses.SickLeave:
                return new Status("Sick Leave");
            case Statuses.SelfIsolation:
                return new Status("Self-Isolation");
            case Statuses.LeaveWithoutPay:
                return new Status("Leave Without Pay");
            default:
                throw new ArgumentException($"Invalid status value: {value}");
        }
    }
}
