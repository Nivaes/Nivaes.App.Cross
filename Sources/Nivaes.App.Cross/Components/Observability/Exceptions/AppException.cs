namespace Nivaes.App.Cross;

public class AppException
    : Exception
{
    public AppException()
       : base()
    { }

    public AppException(string message)
        : base(message)
    { }

    public AppException(string messageFormat, params object?[] messageFormatArguments)
        : base(string.Format(messageFormat, messageFormatArguments))
    {
    }

    public AppException(Exception innerException, string messageFormat, params object?[] formatArguments)
        : base(string.Format(messageFormat, formatArguments), innerException)
    {
    }

    public AppException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
