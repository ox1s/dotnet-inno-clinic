using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Api.Data;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        // System.InvalidOperationException:
        // No backing field could be found for
        // property 'TimeRange.AppointmentId' and
        // the property does not have a getter.

        builder.ToTable("appointments", "appointment");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("appointment_id")
            .ValueGeneratedNever();

        builder.Property(a => a.PatientId)
            .HasColumnName("patient_id");

        builder.Property(a => a.DoctorId)
            .HasColumnName("doctor_id");

        builder.Property(a => a.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();

        builder.Property(a => a.OfficeId)
            .HasColumnName("office_id")
            .IsRequired();

        builder.Property(a => a.TimeRangeId)
            .HasColumnName("time_range_id")
            .IsRequired();

        builder.Property(a => a.IsApproved)
            .HasColumnName("is_approved");

        builder.HasOne(a => a.Time)
            .WithMany()
            .HasForeignKey(a => a.TimeRangeId);
    }
}