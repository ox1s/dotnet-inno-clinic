using ClinicManagement.Api.Authorization;
using ClinicManagement.Api.Database;
using ClinicManagement.Api.Endpoints;
namespace ClinicManagement.Api.ServiceCategories;

public class CreateServiceCategory(
    AppDbContext context)
{
    public sealed record Request(
        string CategoryName,
        int TimeSlotSize);

    public async Task<Guid> Handle(CreateServiceCategory.Request request)
    {

        var category = ServiceCategory.Create(
            request.CategoryName,
            request.TimeSlotSize);
        context.ServiceCategories.Add(category);
        await context.SaveChangesAsync();

        return category.Id;
    }

    internal sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("service-categories", async (CreateServiceCategory.Request request, CreateServiceCategory useCase) =>
                {
                    Guid categoryId = await useCase.Handle(request);
                    return Results.Created($"/service-categories/{categoryId}", categoryId);
                })
                .WithTags(ServiceCategoryEndpoints.Tag)
                .RequirePermission(Permissions.ServicesManipulate);
        }
    }
}