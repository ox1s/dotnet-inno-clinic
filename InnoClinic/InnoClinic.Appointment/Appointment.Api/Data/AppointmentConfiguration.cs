using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Api.Data;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
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

        builder.Property(a => a.Date)
            .HasColumnName("date");

        builder.OwnsOne(a => a.Time, time =>
        {
            time.Property(t => t.Start).HasColumnName("start_time");
            time.Property(t => t.End).HasColumnName("end_time");
        });
    }
}
