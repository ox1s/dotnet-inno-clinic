using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Services;

using FluentValidation;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class CreateOffice(
    AppDbContext context,
    FileUploader fileUploader,
    AbstractValidator<CreateOffice.Request> validator)
{
    public sealed record Request(
        string Address,
        FileStream fileStream,
        string RegistryPhoneNumber,
        bool IsActive);
    public sealed record Response(Guid Id);

    public async Task<Response> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        var photoName = $"{Guid.NewGuid()}.jpg";

        await fileUploader.UploadFileAsync(photoName, request.fileStream, "image/jpg");

        var photo = new Photo(photoName) ??
            throw new ValidationException("The photo is invalid");

        var office = Office.Create(
            request.Address,
            photo,
            request.RegistryPhoneNumber,
            request.IsActive);

        context.Offices.Add(office);
        await context.SaveChangesAsync();

        return new Response(office.Id);
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.RegistryPhoneNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.fileStream)
                .NotNull()
                .Must(fs => fs.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only .jpg files are allowed");
        }
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("offices", async (Request request, CreateOffice useCase) =>
            {
                var response = await useCase.Handle(request);
                return Results.Created($"/offices/{response.Id}", response);
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate);
        }
    }
}