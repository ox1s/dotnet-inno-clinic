using ClinicManagement.Api.Data;
using ClinicManagement.Api.Data.Entities;
using ClinicManagement.Api.Services;

using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Api.Extensions;

public static class DbInitializer
{
    public static async Task InitializeAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var fileUploader = scope.ServiceProvider.GetRequiredService<MinioBlobService>();

        // Initialize Minio bucket
        await fileUploader.GetOrCreateContainerAsync(cancellationToken);

        var configuredCategories = configuration
            .GetSection("CategoriesSettings:Categories")
            .Get<List<CategorySettings>>()?
            .Where(category => !string.IsNullOrWhiteSpace(category.Name) && category.TimeSlotSize > 0)
            .GroupBy(category => category.Name.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group => new CategorySettings
            {
                Name = group.Last().Name.Trim(),
                TimeSlotSize = group.Last().TimeSlotSize
            })
            .ToList();

        if (configuredCategories is null || configuredCategories.Count == 0)
        {
            return;
        }

        var existingCategories = await context.ServiceCategories.ToListAsync(cancellationToken);
        var hasChanges = false;

        foreach (var configuredCategory in configuredCategories)
        {
            var existingCategory = existingCategories.FirstOrDefault(category =>
                string.Equals(category.Name, configuredCategory.Name, StringComparison.OrdinalIgnoreCase));

            if (existingCategory is null)
            {
                context.ServiceCategories.Add(ServiceCategory.Create(
                    configuredCategory.Name,
                    configuredCategory.TimeSlotSize));
                hasChanges = true;
                continue;
            }

            if (existingCategory.TimeSlotSize == configuredCategory.TimeSlotSize)
            {
                continue;
            }

            existingCategory.Update(configuredCategory.Name, configuredCategory.TimeSlotSize);
            hasChanges = true;
        }

        if (hasChanges)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private sealed class CategorySettings
    {
        public string Name { get; init; } = string.Empty;
        public int TimeSlotSize { get; init; }
    }
}
