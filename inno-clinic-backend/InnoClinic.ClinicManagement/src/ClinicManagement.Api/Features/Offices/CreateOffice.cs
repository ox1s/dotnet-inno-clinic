using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class CreateOffice(
    AppDbContext context,
    IBlobService blobService,
    IValidator<CreateOffice.Request> validator)
{
    public sealed record Request(
        string Address,
        IFormFile Photo,
        string RegistryPhoneNumber,
        bool IsActive);
    public sealed record Response(Guid Id);

    public async Task<Response> Handle(Request request)
    {
        await validator.ValidateAndThrowAsync(request);

        using var stream = request.Photo.OpenReadStream();

        var fileId = await blobService.UploadAsync(stream, request.Photo.ContentType);

        var photo = new Photo(fileId.ToString()) ??
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
            RuleFor(x => x.Photo)
                .NotNull()
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/jpg")
                .WithMessage("Only JPEG images are allowed")
                .Must(file => file.Length > 0 && file.Length <= 5 * 1024 * 1024)
                .WithMessage("Photo size must be between 1 byte and 5 MB");
        }
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("offices", async ([FromForm] string address,
                                          [FromForm] IFormFile photo,
                                          [FromForm] string registryPhoneNumber,
                                          [FromForm] bool isActive,
                                          CreateOffice useCase) =>
            {
                var request = new Request(address, photo, registryPhoneNumber, isActive);
                var response = await useCase.Handle(request);
                return Results.Created($"/offices/{response.Id}", response);
            })
            .WithTags(OfficeEndpoints.Tag)
            .RequirePermission(Permissions.OfficesManipulate)
            .DisableAntiforgery();
        }
    }
}