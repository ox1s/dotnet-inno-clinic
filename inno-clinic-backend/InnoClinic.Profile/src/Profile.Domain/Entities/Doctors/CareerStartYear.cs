namespace Profile.Domain.Entities.Doctors;

public record CareerStartYear(int Year)
{
    public static CareerStartYear From(int year)
    {
        return year < 1900 || year > DateTime.Now.Year
            ? throw new ArgumentOutOfRangeException(nameof(year), "Career start year must be between 1900 and the current year.")
            : new CareerStartYear(year);
    }
}