using FluentValidation;

namespace Identity.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(c => c.Password)
            .NotEmpty()
            .Length(6, 15)
            .WithMessage("Password must be between 6 and 15 characters.");
    }
}
