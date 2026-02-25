namespace ClinicManagement.Api.Services;

public sealed record Price(decimal Amount, Currency Currency)
{
    public static Price operator +(Price first, Price second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies have to be equal");
        }

        return new Price(first.Amount + second.Amount, first.Currency);
    }

    public static Price Zero() => new(0, Currency.None);

    public static Price Zero(Currency currency) => new(0, currency);

    public bool IsZero() => this == Zero(Currency);
}
