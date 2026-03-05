using Microsoft.Extensions.Logging;

namespace Profile.Features.Receptionists.Create.CreateDoctorProfile;

public class DoctorCreatedHandler(
    ILogger<DoctorCreatedHandler> logger)
{
    public async Task Handle(DoctorCreated @event)
    {
        logger.LogInformation("Doctor created: {Email}", @event.Email);
        await Task.CompletedTask;
    }
}