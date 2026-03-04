using Profile.Domain.Entities.Patients;

namespace Profile.Infrastructure.Database.Repositories;

internal class PatientRepository : Repository<Patient>
{
    public PatientRepository(ProfileDbContext dbContext) : base(dbContext) { }
}