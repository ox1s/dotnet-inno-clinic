using FluentValidation;
namespace ClinicManagement.Api.Data.Entities;

public sealed record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency Byn = new("BYN");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    /// <summary>
    /// Validating factory for codes arriving from outside (API requests).
    /// </summary>
    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ??
            throw new ValidationException("The currency code is invalid");
    }

    /// <summary>
    /// Read path for the EF value converter. Must never throw: a code that is not in
    /// <see cref="All"/> (including the empty code produced by <see cref="Price.Zero()"/>)
    /// would otherwise make the row impossible to materialise.
    /// </summary>
    public static Currency FromPersistedCode(string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return None;
        }

        return All.FirstOrDefault(c => c.Code == code) ?? new Currency(code);
    }

    public static readonly IReadOnlyCollection<Currency> All =
    [
        Byn,
        Usd,
        Eur
    ];
}
