namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public static class CrossExceptionExtensions
    {
        [Obsolete("", true)]
        public static Exception Wrap(this Exception exception, string message)
        {
            return new CrossException(exception, message);
        }

        [Obsolete("", true)]
        public static Exception Wrap(this Exception exception, string messageFormat, params object?[] formatArguments)
        {
            return new CrossException(exception, messageFormat, formatArguments);
        }
    }
}