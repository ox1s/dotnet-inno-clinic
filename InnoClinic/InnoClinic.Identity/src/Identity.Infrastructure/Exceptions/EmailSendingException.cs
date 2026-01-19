namespace Identity.Infrastructure.Exceptions;

public class EmailSendingException : Exception
{
    public EmailSendingException(string message) : base(message) { }

    public EmailSendingException(string message, Exception innerException)
        : base(message, innerException) { }
}
