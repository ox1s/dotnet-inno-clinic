namespace Profile.Domain.Entities.AccountProfiles;

public record MiddleName(string Value)
{
    public static MiddleName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new MiddleName(value.Trim());
    }
}