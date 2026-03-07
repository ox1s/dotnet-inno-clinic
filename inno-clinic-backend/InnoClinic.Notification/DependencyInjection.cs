using System.Net.Mail;
using System.Text.Json;

using InnoClinic.Notification.Consumers;
using InnoClinic.Notification.Email;

namespace InnoClinic.Notification;

public static class DependencyInjection
{
    public static IServiceCollection AddWebHostServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
        var emailSettings = configuration.GetSection(EmailSettings.Section).Get<EmailSettings>()
            ?? throw new InvalidOperationException("Email settings are missing");

        var mailPitConnectionString = configuration.GetConnectionString("mailpit");
        if (string.IsNullOrEmpty(mailPitConnectionString))
        {
            throw new InvalidOperationException("MailPit connection string is missing");
        }

        mailPitConnectionString = mailPitConnectionString.Replace("Endpoint=", "");
        var uri = new Uri(mailPitConnectionString, UriKind.Absolute);

        services
            .AddFluentEmail(emailSettings.FromEmail, emailSettings.FromName)
            .AddSmtpSender(new SmtpClient(uri.Host, uri.Port));


        services.AddTransient<EmailSender>();
        services.AddHostedService<EmailVerificationConsumer>();
        services.AddHostedService<DoctorCreatedConsumer>();
        services.AddHostedService<SendDailyPollCommandConsumer>();
        services.AddHostedService<TelegramAccountLinkedConsumer>();

        return services;
    }
}