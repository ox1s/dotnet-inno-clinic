using System.Net.Http.Json;

using InnoClinic.Shared.DTOs;

using Microsoft.Extensions.Configuration;
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
    IMessageBus bus,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IJob
{
    private readonly IMongoCollection<Account> _accountsCollection =
        mongoDb.GetDatabase("notifications-db").GetCollection<Account>("accounts");

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Email reminder job started");

        var botToken = configuration["BotSettings:BotToken"];
        if (string.IsNullOrEmpty(botToken))
        {
            logger.LogError("Telegram Bot Token is missing in configuration!");
            return;
        }

        var doctorIds = await doctorRepository.GetGuidsAsync();

        var filter = Builders<Account>.Filter.In(x => x.Id, doctorIds);
        var accounts = await _accountsCollection.Find(filter).ToListAsync();
        var accountsDict = accounts.ToDictionary(a => a.Id);

        using var httpClient = httpClientFactory.CreateClient();
        var telegramApiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";

        foreach (var docId in doctorIds)
        {
            accountsDict.TryGetValue(docId, out var mapping);

            logger.LogInformation("[EmailReminderJob] Processing {AccountId} with TelegramId: {TelegramId}", docId, mapping?.TelegramId);

            if (mapping == null || string.IsNullOrEmpty(mapping.TelegramId))
            {
                var token = docId.ToString("N");
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
                logger.LogInformation("Sending Telegram message directly to {AccountId} (TelegramId: {TelegramId})", docId, mapping.TelegramId);

                var payload = new
                {
                    chat_id = mapping.TelegramId,
                    text = "Пожалуйста, выберите ваш текущий статус на сегодня:",
                    reply_markup = new
                    {
                        inline_keyboard = new[]
                        {
                            new[]
                            {
                                new { text = "🏢 At work", callback_data = "At work" },
                                new { text = "🌴 On vacation", callback_data = "On vacation" }
                            },
                            [
                                new { text = "🤒 Sick Day", callback_data = "Sick Day" },
                                new { text = "🏥 Sick Leave", callback_data = "Sick Leave" }
                            ],
                            [
                                new { text = "🏠 Self-isolation", callback_data = "Self-isolation" },
                                new { text = "🚫 Leave without pay", callback_data = "Leave without pay" }
                            ]
                        }
                    }
                };

                try
                {
                    var response = await httpClient.PostAsJsonAsync(telegramApiUrl, payload);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.LogInformation("Successfully sent Telegram message to {AccountId}", docId);
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        logger.LogWarning("Failed to send Telegram message to {AccountId}. Status: {StatusCode}, Error: {Error}",
                            docId, response.StatusCode, errorContent);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Network error while sending Telegram message to {AccountId}", docId);
                }
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