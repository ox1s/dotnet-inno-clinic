using ClinicManagement.Api.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Api.Data.EntitiesConfigurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    // TODO: Поменять на самоназванные колонки, чтобы не приходилось
    // писать каждый раз кавычки
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsOne(service => service.Price, priceBuilder =>
        {
            priceBuilder.Property(price => price.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.HasOne(service => service.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(service => service.Specialization)
            .WithMany()
            .HasForeignKey(x => x.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}