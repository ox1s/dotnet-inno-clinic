namespace Profile.Domain.Entities.AccountProfiles;

public record FirstName(string Value)
{
    public static FirstName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new FirstName(value.Trim());
    }
}