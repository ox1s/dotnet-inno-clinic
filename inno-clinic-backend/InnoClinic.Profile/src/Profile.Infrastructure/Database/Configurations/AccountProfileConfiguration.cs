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

        builder.HasQueryFilter(a => !a.IsDeleted);

        // account_id is the lookup key used by every "find my profile" path
        // (GetEntityIdByAccountIdAsync, the Telegram bot, the appointments view join).
        // Not unique: existing data may already contain duplicates per account.
        builder.HasIndex(a => a.AccountId);

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

        builder.Property(a => a.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(a => a.DeletedOnUtc)
            .HasColumnName("deleted_on_utc");
    }
}