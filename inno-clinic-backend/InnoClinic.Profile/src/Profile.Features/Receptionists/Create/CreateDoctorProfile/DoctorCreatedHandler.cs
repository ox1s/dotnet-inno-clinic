using Microsoft.Extensions.Logging;

namespace Profile.Features.Receptionists.Create.CreateDoctorProfile;

public class DoctorCreatedHandler(ILogger<DoctorCreatedHandler> logger)
{
    public async Task Handle(DoctorCreated @event)
    {
        logger.LogInformation("Sending welcome email to doctor: {accountId}", @event.AccountId);

        await Task.Delay(1000);
    }
}
