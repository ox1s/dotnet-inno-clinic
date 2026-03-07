using InnoClinic.Shared.DTOs;

using Microsoft.Extensions.Logging;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

using Profile.Infrastructure.Database.Repositories;

using Quartz;

using Wolverine;

namespace Profile.Infrastructure.Services;

public class EmailReminderJob(
    ILogger<EmailReminderJob> logger,
    DoctorRepository doctorRepository,
    IMongoClient mongoDb,
    IMessageBus bus) : IJob
{
    private readonly IMongoCollection<Account> _accountsCollection =
        mongoDb.GetDatabase("notifications-db").GetCollection<Account>("accounts");

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Email reminder job started");

        var doctorIds = await doctorRepository.GetGuidsAsync();

        foreach (var docId in doctorIds)
        {
            var mapping = await _accountsCollection
                                        .Find(m => m.Id == docId)
                                        .FirstOrDefaultAsync();

            if (mapping == null)
            {
                var token = Convert.ToBase64String(docId.ToByteArray());
                var link = $"https://t.me/inno_clinic_bot?start={token}";

                logger.LogInformation("Email begin sending to {AccountId}", docId);
                await bus.PublishAsync(new SendDailyPollCommand
                (
                    docId,
                    DateTime.Now,
                    $"Отметьте статус. Привяжите Telegram для быстрой отметки:",
                    link
                ));
                logger.LogInformation("Email sent to rabbitmq for {AccountId} for the first time", docId);
            }
            else
            {
                logger.LogInformation("Email begin sending to {AccountId}", docId);
                await bus.PublishAsync(new SendDailyPollCommand
                (
                    docId,
                    DateTime.Now,
                    $"Зайдите в Telegram, чтобы отметить статус:",
                    "https://t.me/inno_clinic_bot"
                ));
                logger.LogInformation("Email sent to rabbitmq for {AccountId}", docId);
            }
        }
    }
}
public class Account
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("AccountId")]
    public Guid Id { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("TelegramId")]
    public string? TelegramId { get; set; }
}