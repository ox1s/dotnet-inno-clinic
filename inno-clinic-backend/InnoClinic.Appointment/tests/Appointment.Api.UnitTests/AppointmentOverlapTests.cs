using Appointment.Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.UnitTests;

/// <summary>
/// Regression cover for the doctor overlap check, which had two defects:
/// it filtered on "AND a.is_approved" - and every appointment is created unapproved, so a
/// doctor's pending queue was entirely unguarded against double booking - and it had no way
/// to ignore the appointment being rescheduled, so any reschedule overlapping its own old
/// slot was rejected as a conflict with itself.
///
/// These drive the production predicate (AppointmentRepository.BuildOverlapQuery, which
/// IsOverlappingAsync delegates to) and assert on the SQL the real Npgsql translator emits.
/// ToQueryString compiles without opening a connection. Going through the InMemory provider
/// is not an option: it cannot translate the complex property Appointment.Duration.
/// Row-level coverage belongs in InnoClinic.IntegrationTests, which has a real Postgres.
/// </summary>
public class AppointmentOverlapTests
{
    private static AppointmentRepository NewRepository(out AppointmentDbContext context)
    {
        var options = new DbContextOptionsBuilder<AppointmentDbContext>()
            .UseNpgsql("Host=localhost;Database=translation-only;Username=u;Password=p")
            .Options;

        context = new AppointmentDbContext(options);
        return new AppointmentRepository(context);
    }

    private static TimeRange Range(int startHour, int endHour)
    {
        var day = new DateTimeOffset(2026, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var result = TimeRange.Create(day.AddHours(startHour), day.AddHours(endHour));

        Assert.True(result.Succeeded);
        return result.Value!;
    }

    private static string OverlapSql(Guid? excludeAppointmentId)
    {
        var repository = NewRepository(out var context);

        using (context)
        {
            return repository
                .BuildOverlapQuery(Guid.NewGuid(), Range(9, 10), excludeAppointmentId)
                .ToQueryString();
        }
    }

    /// <summary>
    /// Restricts assertions to the predicate. IsOverlappingAsync calls AnyAsync, which projects
    /// no columns, so matching against the whole statement would hit the SELECT list instead.
    /// </summary>
    private static string OverlapPredicate(Guid? excludeAppointmentId)
    {
        var sql = OverlapSql(excludeAppointmentId);
        var whereIndex = sql.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);

        Assert.True(whereIndex >= 0, $"Expected a WHERE clause in:{Environment.NewLine}{sql}");

        return sql[whereIndex..];
    }

    [Fact]
    public void OverlapQuery_TargetsTheAppointmentsTableNotTheCrossSchemaView()
    {
        var sql = OverlapSql(excludeAppointmentId: null);

        Assert.Contains("appointment.appointments", sql, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("appointments_view", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void OverlapQuery_DoesNotFilterOnApprovalState()
    {
        var predicate = OverlapPredicate(excludeAppointmentId: null);

        // The whole point of the fix: a pending appointment still blocks the slot.
        Assert.DoesNotContain("is_approved", predicate, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void OverlapQuery_UsesHalfOpenIntervalComparison()
    {
        var predicate = OverlapPredicate(excludeAppointmentId: null);

        // Strict < / > so a slot starting exactly when the previous one ends stays free.
        Assert.Contains("duration_start <", predicate, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("duration_end >", predicate, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("duration_start <=", predicate, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("duration_end >=", predicate, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void OverlapQuery_FiltersByDoctor()
    {
        var predicate = OverlapPredicate(excludeAppointmentId: null);

        Assert.Contains("doctor_id", predicate, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void OverlapQuery_ExcludesTheRescheduledAppointmentOnlyWhenAsked()
    {
        var withoutExclusion = OverlapPredicate(excludeAppointmentId: null);
        var withExclusion = OverlapPredicate(excludeAppointmentId: Guid.NewGuid());

        Assert.DoesNotContain("appointment_id <>", withoutExclusion, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("appointment_id <>", withExclusion, StringComparison.OrdinalIgnoreCase);
    }
}
