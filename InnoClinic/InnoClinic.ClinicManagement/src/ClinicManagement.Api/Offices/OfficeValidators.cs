using FluentValidation;

namespace ClinicManagement.Api.Offices;

internal sealed class CreateOfficeValidator : AbstractValidator<CreateOffice.Request>
{
    public CreateOfficeValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty()
            .Matches(@"^\+375\d{9}$")
            .WithMessage("Phone number must be in Belarusian format (e.g., +375291234567).");
    }
}

internal sealed class UpdateOfficeValidator : AbstractValidator<UpdateOffice.Request>
{
    public UpdateOfficeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty()
            .Matches(@"^\+375\d{9}$")
            .WithMessage("Phone number must be in Belarusian format (e.g., +375291234567).");
    }
}