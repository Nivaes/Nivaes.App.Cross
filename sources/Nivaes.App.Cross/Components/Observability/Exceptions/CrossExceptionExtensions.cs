namespace Nivaes.App.Cross
{
    public static class CrossExceptionExtensions
    {
        public static Exception Wrap(this Exception exception)
        {
            if (exception is CrossException)
                return exception;

            return Wrap(exception, exception.Message);
        }

        public static Exception Wrap(this Exception exception, string message)
        {
            return new CrossException(exception, message);
        }

        public static Exception Wrap(this Exception exception, string messageFormat, params object?[] formatArguments)
        {
            return new CrossException(exception, messageFormat, formatArguments);
        }
    }
}