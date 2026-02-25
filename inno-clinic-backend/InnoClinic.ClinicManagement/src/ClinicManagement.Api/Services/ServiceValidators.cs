using FluentValidation;

namespace ClinicManagement.Api.Services;

internal sealed class CreateServiceValidator : AbstractValidator<CreateService.Request>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Service name is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Service category is required.");
    }
}

internal sealed class UpdateServiceValidator : AbstractValidator<UpdateService.Request>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.ServiceName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.SpecializationId)
            .NotEmpty();
    }
}