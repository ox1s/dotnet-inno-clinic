using System.Text.RegularExpressions;
using FluentValidation;

namespace Identity.Application.Authentication.Commands.Register;

public partial class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private static readonly Regex PasswordRegex = StrongPasswordRegex();

    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(c => c.Password)
            .NotEmpty()
            .Length(6, 15)
            .WithMessage("Password must be between 6 and 15 characters.")
            .Matches(PasswordRegex)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,15}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();
}
