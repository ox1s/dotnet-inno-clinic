using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Futures.Appointments;

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
}
