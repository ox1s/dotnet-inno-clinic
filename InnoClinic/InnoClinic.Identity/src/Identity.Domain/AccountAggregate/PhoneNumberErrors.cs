using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public static class PhoneNumberErrors
{
    public static readonly Error Invalid = Error.Validation(
        "PhoneNumber.Invalid",
        Resources.PhoneNumber_Invalid);

    public static readonly Error WrongCountry = Error.Validation(
        "PhoneNumber.WrongCountry",
        Resources.PhoneNumber_WrongCountry);

    public static readonly Error ParseError = Error.Validation(
        "PhoneNumber.ParseError",
        Resources.PhoneNumber_ParseError);
}