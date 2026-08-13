using FluentValidation;

namespace ClinicManagement.Api.Features.Offices;

// Single home for office validation. Keeping one AbstractValidator per request type is
// deliberate: AddValidatorsFromAssembly registers every implementation it finds, and a
// handler injecting a single IValidator<T> only ever runs one of them.
internal static class OfficeValidationRules
{
    // Must stay <= the column width configured in OfficeConfiguration.
    internal const int AddressMaxLength = 500;
    internal const string BelarusianPhonePattern = @"^\+375\d{9}$";
    internal const long PhotoMaxBytes = 5 * 1024 * 1024;

    internal static readonly string[] AllowedPhotoContentTypes =
    [
        "image/jpeg",
        "image/jpg"
    ];
}

internal sealed class CreateOfficeValidator : AbstractValidator<CreateOffice.Request>
{
    public CreateOfficeValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(OfficeValidationRules.AddressMaxLength);

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty()
            .Matches(OfficeValidationRules.BelarusianPhonePattern)
            .WithMessage("Phone number must be in Belarusian format (e.g., +375291234567).");

        RuleFor(x => x.Photo)
            .NotNull()
            .WithMessage("A photo is required.");

        RuleFor(x => x.Photo)
            .Must(file => OfficeValidationRules.AllowedPhotoContentTypes.Contains(file.ContentType))
            .WithMessage("Only JPEG images are allowed")
            .Must(file => file.Length > 0 && file.Length <= OfficeValidationRules.PhotoMaxBytes)
            .WithMessage("Photo size must be between 1 byte and 5 MB")
            .When(x => x.Photo is not null);
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
            .MaximumLength(OfficeValidationRules.AddressMaxLength);

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty()
            .Matches(OfficeValidationRules.BelarusianPhonePattern)
            .WithMessage("Phone number must be in Belarusian format (e.g., +375291234567).");
    }
}
