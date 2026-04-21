using FluentEmail.Core;

using InnoClinic.Notification.Exceptions;

namespace InnoClinic.Notification.Email;

public partial class EmailSender(
    IFluentEmail email,
    ILogger<EmailSender> logger)
{
    public async Task SendEmailAsync(string to, string from, string subject, string body)
    {
        LogSendingEmailToToWithSubjectSubject(logger, to, subject);

        var fluentEmail = email
            .To(to)
            .Subject(subject)
            .Body(body, true);

        if (!string.IsNullOrEmpty(from)) fluentEmail.SetFrom(from);

        var response = await fluentEmail.SendAsync();

        if (!response.Successful)
        {
            var errors = string.Join(", ", response.ErrorMessages);
            LogFailedToSendEmailErrorsErrors(logger, errors);

            throw new EmailSendingException($"Failed to send email: {errors}");
        }

        LogEmailSentSuccessfully(logger);
    }

    [LoggerMessage(LogLevel.Information, "Sending email to {To} with subject {Subject}")]
    static partial void LogSendingEmailToToWithSubjectSubject(ILogger<EmailSender> logger, string To, string Subject);

    [LoggerMessage(LogLevel.Error, "Failed to send email. Errors: {Errors}")]
    static partial void LogFailedToSendEmailErrorsErrors(ILogger<EmailSender> logger, string Errors);

    [LoggerMessage(LogLevel.Information, "Email sent successfully!")]
    static partial void LogEmailSentSuccessfully(ILogger<EmailSender> logger);
}