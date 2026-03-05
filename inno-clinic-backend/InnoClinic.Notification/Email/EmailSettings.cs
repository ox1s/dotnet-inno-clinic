namespace InnoClinic.Notification.Email;

public class EmailSettings
{
    public const string Section = "EmailSettings";

    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;

    public string WelcomeSubject { get; set; } = string.Empty;
    public string WelcomeBodyTemplate { get; set; } = string.Empty;
}