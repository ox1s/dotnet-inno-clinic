using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public static class PhoneNumberErrors
{
    public static readonly Error Invalid = Error.Validation(
        "PhoneNumber.Invalid",
        Identity_Resources.PhoneNumber_Invalid);

    public static readonly Error WrongCountry = Error.Validation(
        "PhoneNumber.WrongCountry",
        Identity_Resources.PhoneNumber_WrongCountry);

    public static readonly Error ParseError = Error.Validation(
        "PhoneNumber.ParseError",
        Identity_Resources.PhoneNumber_ParseError);
}