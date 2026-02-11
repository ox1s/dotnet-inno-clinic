using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Data;

public class AppointmentDbContext(
    DbContextOptions<AppointmentDbContext> options)
    : DbContext(options)
{
    public DbSet<Appointment> Appointments { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("appointment");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public async Task<bool> IsOverlappingAsync(
        Guid doctorId,
        TimeRange duration,
        CancellationToken cancellationToken = default)
    {
        return await Appointments
            .AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.Duration.Start < duration.End &&
                a.Duration.End > duration.Start
                // TODO: && a.IsApproved
                , 
                cancellationToken);
    }

}