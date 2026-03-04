using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Profile.Domain.Entities.AccountProfiles;
namespace Profile.Infrastructure.Database.Configurations;

public class AccountProfileConfiguration : IEntityTypeConfiguration<AccountProfile>
{

    public void Configure(EntityTypeBuilder<AccountProfile> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("account_profile_id")
            .ValueGeneratedNever();

        builder.Property(a => a.AccountId)
            .HasColumnName("account_id");

        builder.Property(p => p.FirstName)
            .HasConversion(
                v => v.Value,
                v => new FirstName(v))
            .HasColumnName("first_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.LastName)
            .HasConversion(
                v => v.Value,
                v => new LastName(v))
            .HasColumnName("last_name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.MiddleName)
            .HasConversion(
                v => v.Value,
                v => new MiddleName(v))
            .HasColumnName("middle_name")
            .HasMaxLength(50);
    }
}
