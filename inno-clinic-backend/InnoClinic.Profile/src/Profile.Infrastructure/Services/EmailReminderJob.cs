// using Microsoft.Extensions.Logging;
//
// using Quartz;
//
// using Telegram.Bot;
// using Telegram.Bot.Types.ReplyMarkups;
//
// namespace Profile.Infrastructure.Services;
//
// public class EmailReminderJob(ILogger<EmailReminderJob> logger, IEmailService emailService) : IJob
// {
//     public const string Name = nameof(EmailReminderJob);
//
//     public async Task Execute(IJobExecutionContext context)
//     {
//         // Best practice: Prefer using MergedJobDataMap
//         var data = context.MergedJobDataMap;
//
//         // Get job data - note that this isn't strongly typed
//         string? userId = data.GetString("userId");
//         string? message = data.GetString("message");
//
//         try
//         {
//             await emailService.SendReminderAsync(userId, message);
//
//             logger.LogInformation("Sent reminder to user {UserId}: {Message}", userId, message);
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Failed to send reminder to user {UserId}", userId);
//
//             // Rethrow to let Quartz handle retry logic
//             throw;
//         }
//     }
// }
