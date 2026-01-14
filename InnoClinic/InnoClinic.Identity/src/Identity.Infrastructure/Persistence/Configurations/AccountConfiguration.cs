using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.Domain.AccountAggregate;

namespace Identity.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.Email)
            .HasMaxLength(400)
            .IsRequired()
            .HasColumnName("email")
            .HasConversion(
                email => email.Value,
                value => new Email(value));

        builder.HasIndex(a => a.Email)
            .IsUnique();

        builder.Property(a => a.IsEmailVerified);

        builder.Property(a => a.EmailVerificationToken)
            .HasColumnName("email_verification_token");

        builder.Property(a => a.EmailVerificationTokenExpiration)
            .HasColumnName("email_verification_token_expiration");

        builder.Property(a => a.PhotoId)
            .HasColumnName("photo_id");

        builder.OwnsOne(a => a.CreatedInfo, ci =>
        {
            ci.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            ci.Property(c => c.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();
        });

        builder.OwnsOne(a => a.UpdatedInfo, ui =>
        {
            ui.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at");

            ui.Property(u => u.UpdatedBy)
                .HasColumnName("updated_by");
        });

        builder.Property("_passwordHash")
            .HasColumnName("password_hash")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
