using InnoClinic.Shared;

namespace Profile.Domain.Entities.Doctors;

public record Status(string Value)
{
    private static readonly string[] Allowed =
    [
        Statuses.AtWork,
        Statuses.OnVacation,
        Statuses.SickDay,
        Statuses.SickLeave,
        Statuses.SelfIsolation,
        Statuses.LeaveWithoutPay
    ];

    /// <summary>
    /// Validating factory for values arriving from outside the domain (API, bot, message bus).
    /// Throws on anything that is not one of <see cref="Statuses"/>.
    /// </summary>
    public static Status From(string value)
    {
        var canonical = Canonicalize(value)
            ?? throw new ArgumentException($"Invalid status value: {value}");

        return new Status(canonical);
    }

    /// <summary>
    /// Read path for the EF value converter. Must never throw: a value that cannot be
    /// recognised would otherwise make the whole row - and any page containing it -
    /// impossible to materialise. Legacy values that differ only by casing are
    /// canonicalised, anything else is passed through verbatim so the row stays readable.
    /// </summary>
    public static Status FromPersisted(string value)
    {
        return new Status(Canonicalize(value) ?? value);
    }

    private static string? Canonicalize(string value)
    {
        return Array.Find(Allowed, allowed =>
            string.Equals(allowed, value, StringComparison.OrdinalIgnoreCase));
    }
}
