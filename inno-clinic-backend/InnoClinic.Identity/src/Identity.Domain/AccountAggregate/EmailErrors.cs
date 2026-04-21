using ErrorOr;

namespace Identity.Domain.AccountAggregate;

public static class EmailErrors
{
    // Given the email field is empty And the field loses focusThen the border of the field becomes red And an error message of a missing value is shown to the User “Please, enter the email”
    // Given the field doesn’t contain @And the field loses focusThen the border of the field becomes red And an error message is shown to the User “You've entered an invalid email”
    // Given the email exists in the systemAnd the field loses focus Then the border of the field becomes red And an error message is shown to the User “User with this email already exists”

    public static readonly Error Empty = Error.Validation(
        "Email.Empty",
        Identity_Resources.Email_Empty); // Frontend: "Please, enter the email"

    public static readonly Error InvalidFormat = Error.Validation(
        "Email.InvalidFormat",
        Identity_Resources.Email_InvalidFormat); // Frontend: "You've entered an invalid email"

    public static readonly Error TooLong = Error.Validation(
        "Email.Invalids",
        Identity_Resources.Email_TooLong);

}