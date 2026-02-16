using ErrorOr;

using PhoneNumbers;

namespace Identity.Domain.AccountAggregate;

public sealed record PhoneNumber(string Value)
{
    public static ErrorOr<PhoneNumber> Create(string rawNumber)
    {
        var phoneUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var parsed = phoneUtil.Parse(rawNumber, "BY");
            var actualRegion = phoneUtil.GetRegionCodeForNumber(parsed);
            if (actualRegion != "BY") return PhoneNumberErrors.WrongCountry;
            var normalized = phoneUtil.Format(parsed, PhoneNumberFormat.E164);
            return new PhoneNumber(normalized);
        }
        catch (NumberParseException)
        {
            return PhoneNumberErrors.ParseError;
        }
    }
}