using InnoClinic.Shared;

using Microsoft.EntityFrameworkCore;

using Profile.Domain.Entities.Doctors;

namespace Profile.Infrastructure.Database.Repositories;

public class DoctorRepository : Repository<Doctor>
{
    public DoctorRepository(ProfileDbContext dbContext) : base(dbContext) { }

    public async Task<bool> IsDoctorActiveAsync(Guid doctorId)
    {
        var doctor = await GetByIdAsync(doctorId);
        return doctor != null && doctor.Status == Status.From(Statuses.AtWork);
    }
    public async Task<IEnumerable<Guid>> GetGuidsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Doctor>()
            .AsNoTracking()
            .Select(d => d.Id)
            .ToListAsync(cancellationToken);
    }
}
