using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profile.Domain.Entities.Doctors;
namespace Profile.Infrastructure.Database.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{

    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("doctors", "profile");

        builder.Property(d => d.DateOfBirth)
            .HasColumnName("date_of_birth");

        builder.Property(d => d.SpecializationId)
            .HasColumnName("specialization_id");

        builder.Property(d => d.OfficeId)
            .HasColumnName("office_id");

        builder.Property(d => d.CareerStartYear)
               .HasConversion(v => v.Year, v => CareerStartYear.From(v))
               .HasColumnName("career_start_year");

        builder.Property(d => d.Status)
                .HasConversion(v => v.Value, v => Status.From(v))
                .HasColumnName("status");
    }
}