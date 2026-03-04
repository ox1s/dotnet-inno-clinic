using Profile.Domain.Entities.Doctors;

namespace Profile.Infrastructure.Database.Repositories;

internal class DoctorRepository : Repository<Doctor>
{
    public DoctorRepository(ProfileDbContext dbContext) : base(dbContext) { }

    public async Task<bool> IsDoctorActiveAsync(Guid doctorId)
    {
        var doctor = await GetByIdAsync(doctorId);
        return doctor != null && doctor.Status == "At work";
    }
}
