using Appointment.Api.Features.Receptionist.ListAppointments;

using Microsoft.EntityFrameworkCore;

namespace Appointment.Api.Data;

public class AppointmentRepository(AppointmentDbContext dbContext)
    : IAppointmentRepository
{
    private readonly AppointmentDbContext _dbContext = dbContext;

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _dbContext.Appointments
            .Where(a => a.Id == appointment.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _dbContext.Appointments.Update(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsOverlappingAsync(
        Guid doctorId,
        TimeRange duration,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.AppointmentViews
            .AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.DurationStart < duration.End &&
                a.DurationEnd > duration.Start &&
                a.IsApproved,
                cancellationToken);
    }

    private IQueryable<AppointmentView> BuildQuery(AppointmentFilter filter)
    {
        var query = _dbContext.AppointmentViews.AsNoTracking();

        if (filter.DoctorId.HasValue)
            query = query.Where(a => a.DoctorId == filter.DoctorId.Value);

        if (filter.PatientId.HasValue)
            query = query.Where(a => a.PatientId == filter.PatientId.Value);

        if (filter.ServiceId.HasValue)
            query = query.Where(a => a.ServiceId == filter.ServiceId.Value);

        if (filter.OfficeId.HasValue)
            query = query.Where(a => a.OfficeId == filter.OfficeId.Value);

        if (filter.Date.HasValue)
            query = query.Where(a => a.LocalDate == filter.Date.Value);

        if (filter.DateStart.HasValue)
            query = query.Where(a => a.LocalDate >= filter.DateStart.Value);

        if (filter.DateEnd.HasValue)
            query = query.Where(a => a.LocalDate <= filter.DateEnd.Value);

        if (filter.IsApproved.HasValue)
            query = query.Where(a => a.IsApproved == filter.IsApproved.Value);

        return query;
    }

    public async Task<int> CountAsync(AppointmentFilter filter, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(filter);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<AppointmentView>> SearchAsync(AppointmentFilter filter, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(filter);
        query = ApplySorting(query, filter)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize);

        return await query.ToListAsync(cancellationToken);
    }
    private static IQueryable<AppointmentView> ApplySorting(
        IQueryable<AppointmentView> query,
        AppointmentFilter filter)
    {
        return (filter.SortBy, filter.SortDirection) switch
        {
            (SortOptions.ServiceName, SortDirection.Desc) =>
                query.OrderByDescending(x => x.ServiceName),

            (SortOptions.ServiceName, SortDirection.Asc) =>
                query.OrderBy(x => x.ServiceName),

            (SortOptions.DoctorName, SortDirection.Desc) =>
                query.OrderByDescending(x => x.DoctorLastName)
                     .ThenByDescending(x => x.DoctorFirstName),

            (SortOptions.DoctorName, SortDirection.Asc) =>
                query.OrderBy(x => x.DoctorLastName)
                     .ThenBy(x => x.DoctorFirstName),

            (SortOptions.Date, SortDirection.Desc) =>
                query.OrderByDescending(x => x.DurationStart),

            _ =>
                query.OrderBy(x => x.DurationStart)
                    .ThenBy(x => x.DoctorLastName)
                    .ThenBy(x => x.DoctorFirstName)
                    .ThenBy(x => x.ServiceName),
        };
    }
}