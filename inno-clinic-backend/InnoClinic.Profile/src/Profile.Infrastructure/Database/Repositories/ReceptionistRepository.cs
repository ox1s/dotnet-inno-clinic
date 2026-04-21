using Profile.Domain.Entities.Receptionists;

namespace Profile.Infrastructure.Database.Repositories;

internal class ReceptionistRepository : Repository<Receptionist>
{
    public ReceptionistRepository(ProfileDbContext dbContext) : base(dbContext) { }
}