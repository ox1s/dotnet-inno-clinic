namespace Identity.Infrastructure.Services.Email;

public class EmailSettings
{
    public const string Section = "EmailSettings";

    public string FromEmail { get; set; } = "no-reply@innoclinic.com";
    public string FromName { get; set; } = "InnoClinic Service";
}
