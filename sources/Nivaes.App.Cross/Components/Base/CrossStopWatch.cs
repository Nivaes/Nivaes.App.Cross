namespace Nivaes.App.Cross
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross.Observability;

    [Obsolete("", true)]
    public sealed class CrossStopWatch
        : IDisposable
    {
        //private readonly ILogger? _log;
        private readonly string _message;
        private readonly int _startTickCount;

        private CrossStopWatch(string text, params object[] args)
        {
            //_log = CrossLogHost.GetLog<CrossStopWatch>();
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private CrossStopWatch(string tag, string text, params object[] args)
        {
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        public void Dispose()
        {

            CrossLoggerHost.GetLogger<CrossStopWatch>().Log(LogLevel.Trace, "{Ticks} - {Message}", Environment.TickCount - _startTickCount, _message);
            GC.SuppressFinalize(this);
        }

        public static CrossStopWatch Create(string text, params object[] args)
        {
            return new CrossStopWatch(text, args);
        }

        public static CrossStopWatch CreateWithTag(string tag, string text, params object[] args)
        {
            return new CrossStopWatch(tag, text, args);
        }
    }
}
