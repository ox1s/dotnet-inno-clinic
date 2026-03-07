using Microsoft.AspNetCore.Http;

using Profile.Domain.Entities.Doctors;
using Profile.Infrastructure.Database;
using Profile.Infrastructure.Database.Repositories;

namespace Profile.Features.Doctors.EditDoctorStatusByBot;

public static class EditDoctorStatusByBotCommandHandler
{
    public static async Task Handle(
        EditDoctorStatusByBotCommand command,
        ProfileDbContext dbContext,
        DoctorRepository doctorRepository,
        AccountRepository accountRepository)
    {
        var entityId = await accountRepository.GetEntityIdByAccountIdAsync(command.AccountId);
        if (entityId is null) throw new ArgumentException("Entity not found");

        var doctor = await doctorRepository
            .GetByIdAsync(entityId.Value);

        if (doctor is null) throw new ArgumentException("Doctor not found");
        doctor.Status = Status.From(command.Status);

        await dbContext.SaveChangesAsync();
    }
}