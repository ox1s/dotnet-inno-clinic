using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profile.Domain.Entities.Receptionists;
namespace Profile.Infrastructure.Database.Configurations;

public class ReceptionistConfiguration : IEntityTypeConfiguration<Receptionist>
{

    public void Configure(EntityTypeBuilder<Receptionist> builder)
    {
        builder.ToTable("receptionists", "profile");

        builder.HasIndex(r => r.OfficeId);

        builder.Property(r => r.OfficeId)
            .HasColumnName("office_id");
    }
}