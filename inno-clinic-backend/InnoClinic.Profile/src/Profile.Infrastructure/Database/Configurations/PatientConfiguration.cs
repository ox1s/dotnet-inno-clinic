using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profile.Domain.Entities.Patients;
namespace Profile.Infrastructure.Database.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(
        EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients", "profile");

        builder.Property(p => p.IsLinkedToAccount)
            .HasColumnName("is_linked_to_account");

        builder.Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth");
    }
}