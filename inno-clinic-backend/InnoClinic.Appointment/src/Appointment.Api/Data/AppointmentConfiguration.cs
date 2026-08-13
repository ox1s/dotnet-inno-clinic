using System.Reflection.Emit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Api.Data;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments", "appointment");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("appointment_id")
            .ValueGeneratedNever();

        builder.Property(a => a.PatientId)
            .HasColumnName("patient_id");

        // The overlap check filters on doctor_id before narrowing by time range, and the
        // schedule/history queries filter on patient_id. Neither had any index.
        builder.HasIndex(a => a.DoctorId);
        builder.HasIndex(a => a.PatientId);

        builder.Property(a => a.DoctorId)
            .HasColumnName("doctor_id");

        builder.Property(a => a.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();

        builder.Property(a => a.OfficeId)
            .HasColumnName("office_id")
            .IsRequired();

        builder.Property(a => a.IsApproved)
            .HasColumnName("is_approved");

        builder.ComplexProperty(e => e.Duration,
            d =>
            {
                d.Property(e => e.Start).HasColumnName("duration_start");
                d.Property(e => e.End).HasColumnName("duration_end");
            });
    }
}