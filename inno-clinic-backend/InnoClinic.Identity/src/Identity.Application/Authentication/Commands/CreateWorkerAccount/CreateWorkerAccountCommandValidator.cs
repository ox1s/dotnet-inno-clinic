using System.Text.RegularExpressions;

using FluentValidation;

using InnoClinic.Shared;

namespace Identity.Application.Authentication.Commands.CreateWorkerAccount;

public partial class CreateWorkerAccountCommandValidator : AbstractValidator<CreateWorkerAccountCommand>
{
    private static readonly Regex _passwordRegex = StrongPasswordRegex();

    public CreateWorkerAccountCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(c => c.Password)
            .NotEmpty()
            .Length(6, 15)
            .WithMessage("Password must be between 6 and 15 characters.")
            .Matches(_passwordRegex)
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character.");

        RuleFor(c => c.Role)
            .Must(BeSupportedWorkerRole)
            .WithMessage($"Role must be either {Roles.Doctor} or {Roles.Receptionist}.");
    }

    private static bool BeSupportedWorkerRole(string role)
    {
        return string.Equals(role, Roles.Doctor, StringComparison.OrdinalIgnoreCase)
            || string.Equals(role, Roles.Receptionist, StringComparison.OrdinalIgnoreCase);
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,15}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();
}