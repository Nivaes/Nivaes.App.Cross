namespace Nivaes.App.Cross
{
    using System;

    public class CrossException 
        : Exception
    {
        public CrossException()
        {
        }

        public CrossException(string message)
            : base(message)
        {
        }

        public CrossException(string messageFormat, params object?[] messageFormatArguments)
            : base(string.Format(messageFormat, messageFormatArguments))
        {
        }

        // the order of parameters here is slightly different to that normally expected in an exception
        // - but this order allows us to put string.Format in place
        public CrossException(Exception innerException, string messageFormat, params object?[] formatArguments)
            : base(string.Format(messageFormat, formatArguments), innerException)
        {
        }

        public CrossException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
