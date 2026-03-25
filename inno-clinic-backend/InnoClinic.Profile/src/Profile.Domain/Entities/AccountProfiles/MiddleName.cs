namespace Profile.Domain.Entities.AccountProfiles;

public record MiddleName(string Value)
{
    public static MiddleName Create(string value)
    {
        // Middle name is optional in requirements.
        if (string.IsNullOrWhiteSpace(value))
        {
            return new MiddleName(string.Empty);
        }

        return new MiddleName(value.Trim());
    }
}
