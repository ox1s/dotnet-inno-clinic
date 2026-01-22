using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Features.Appointments;

public class AppointmentDbContext(
    DbContextOptions<AppointmentDbContext> options)
    : DbContext(options)
{
    public DbSet<Data.Appointment> Appointments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("appointment");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
