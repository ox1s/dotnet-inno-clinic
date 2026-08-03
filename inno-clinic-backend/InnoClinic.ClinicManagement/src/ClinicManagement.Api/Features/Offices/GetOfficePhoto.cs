using ClinicManagement.Api.Data;
using ClinicManagement.Api.Endpoints;
using ClinicManagement.Api.Services;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Features.Offices;

internal sealed class GetOfficePhoto(AppDbContext context, IBlobService blobService)
{
    public sealed record Response(Stream Stream, string ContentType);
    public async Task<Response?> Handle(Guid id, CancellationToken cancellationToken)
    {
        var office = await context.Offices
            .AsNoTracking()
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (office is null)
        {
            return null;
        }

        var photoResponse = await blobService.DownloadAsync(Guid.Parse(office.Photo.Url), cancellationToken);

        return new Response(photoResponse.Stream, photoResponse.ContentType);
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("offices/{id:guid}/photo", async (Guid id, GetOfficePhoto useCase, CancellationToken ct) =>
            {
                var response = await useCase.Handle(id, ct);
                
                return response is null ? 
                    Results.NotFound() 
                    : Results.File(response.Stream, response.ContentType);
            })
            .WithTags(OfficeEndpoints.Tag)
            .AllowAnonymous();
        }
    }
}
