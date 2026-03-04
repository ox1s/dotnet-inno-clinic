namespace Profile.Domain.Entities.AccountProfiles;

public record LastName(string Value)
{
    public static LastName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        return new LastName(value.Trim());
    }
}