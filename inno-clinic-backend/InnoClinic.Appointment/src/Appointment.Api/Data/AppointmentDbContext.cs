using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Data;

public class AppointmentDbContext(
    DbContextOptions<AppointmentDbContext> options)
    : DbContext(options)
{
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<AppointmentView> AppointmentViews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("appointment");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppointmentView>(eb =>
        {
            eb.HasNoKey();
            eb.ToView("appointments_view", "appointment");
            eb.Property(v => v.AppointmentId).HasColumnName("appointment_id");
            eb.Property(v => v.LocalDate).HasColumnName("local_date");
            eb.Property(v => v.DurationStart).HasColumnName("duration_start");
            eb.Property(v => v.DurationEnd).HasColumnName("duration_end");
            eb.Property(v => v.IsApproved).HasColumnName("is_approved");

            eb.Property(v => v.DoctorId).HasColumnName("doctor_id");
            eb.Property(v => v.DoctorFirstName).HasColumnName("doctor_first_name");
            eb.Property(v => v.DoctorLastName).HasColumnName("doctor_last_name");
            eb.Property(v => v.DoctorMiddleName).HasColumnName("doctor_middle_name");

            eb.Property(v => v.PatientId).HasColumnName("patient_id");
            eb.Property(v => v.PatientFirstName).HasColumnName("patient_first_name");
            eb.Property(v => v.PatientLastName).HasColumnName("patient_last_name");
            eb.Property(v => v.PatientPhone).HasColumnName("patient_phone");

            eb.Property(v => v.ServiceId).HasColumnName("service_id");
            eb.Property(v => v.ServiceName).HasColumnName("service_name");
        });
    }

}