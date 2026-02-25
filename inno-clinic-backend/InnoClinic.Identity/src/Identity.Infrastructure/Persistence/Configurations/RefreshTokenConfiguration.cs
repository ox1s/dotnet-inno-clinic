using Identity.Domain.AccountAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(
        EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens", "identity");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("refresh_token_id")
            .ValueGeneratedNever();

        builder.Property(a => a.Token)
            .HasColumnName("token")
            .HasMaxLength(200)
            .IsRequired();
        builder.HasIndex(a => a.Token)
            .IsUnique();

        builder.HasOne(a => a.Account)
            .WithMany().HasForeignKey(a => a.AccountId);
    }
}