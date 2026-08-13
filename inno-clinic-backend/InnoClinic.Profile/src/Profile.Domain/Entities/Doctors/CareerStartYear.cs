namespace Profile.Domain.Entities.Doctors;

public record CareerStartYear(int Year)
{
    /// <summary>
    /// Validating factory for values arriving from outside the domain.
    /// </summary>
    public static CareerStartYear From(int year)
    {
        return year < 1900 || year > DateTime.UtcNow.Year
            ? throw new ArgumentOutOfRangeException(nameof(year), "Career start year must be between 1900 and the current year.")
            : new CareerStartYear(year);
    }

    /// <summary>
    /// Read path for the EF value converter - must never throw, otherwise an out-of-range
    /// value already in the database would make the row impossible to materialise.
    /// </summary>
    public static CareerStartYear FromPersisted(int year) => new(year);
}
