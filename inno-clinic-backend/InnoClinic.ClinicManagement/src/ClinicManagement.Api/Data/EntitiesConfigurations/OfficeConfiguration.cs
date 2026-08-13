using ClinicManagement.Api.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Api.Data.EntitiesConfigurations;

public class OfficeConfiguration : IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.HasKey(x => x.Id);

        // Must stay in sync with OfficeValidationRules.AddressMaxLength, otherwise an address
        // that passes validation is rejected by Postgres (22001) as a 500 instead of a 400.
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RegistryPhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.ComplexProperty(property => property.Photo);
    }
}