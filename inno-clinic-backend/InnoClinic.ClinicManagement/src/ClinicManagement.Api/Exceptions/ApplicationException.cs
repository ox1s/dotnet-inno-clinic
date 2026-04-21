namespace ClinicManagement.Api.Exceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException(string message)
        : base(message)
    {
    }
}