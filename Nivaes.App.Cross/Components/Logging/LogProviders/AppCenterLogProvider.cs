// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;

    internal sealed class AppCenterLogProvider
        : MvxBaseLogProvider
    {
        protected override Logger GetLogger(string name) => new AppCenterLogger(name).Log;

        //private static string MessageFormatter(string loggerName, MvxLogLevel level, object message, Exception? exception)
        //{
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        //    stringBuilder.Append(' ');

        //    // Append a readable representation of the log level
        //    _ = stringBuilder.Append($"[{level.ToString().ToUpper(CultureInfo.DefaultThreadCurrentCulture)}]".PadRight(8));
        //    _ = stringBuilder.Append($"({loggerName}) ");

        //    // Append the message
        //    stringBuilder.Append(message);

        //    // Append stack trace if there is an exception
        //    if (exception != null)
        //    {
        //        stringBuilder.Append(Environment.NewLine).Append(exception.GetType());
        //        stringBuilder.Append(Environment.NewLine).Append(exception.Message);
        //        stringBuilder.Append(Environment.NewLine).Append(exception.StackTrace);
        //    }

        //    return stringBuilder.ToString();
        //}

        private class AppCenterLogger
        {
            private readonly string mName;

            public AppCenterLogger(string name)
            {
                mName = name;
            }

            public bool Log(MvxLogLevel logLevel, Func<string>? messageFunc, Exception? exception,
                params object[] formatParameters)
            {
                if (messageFunc == null) return true;

                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);

                var message = messageFunc();

                //Write(logLevel, messageFunc(), exception);

                if (logLevel == MvxLogLevel.Error || logLevel == MvxLogLevel.Fatal)
                {
                    Crashes.TrackError(exception);
                }
                else
                {
                    Analytics.TrackEvent(message);
                }

                return true;
            }

            //protected void Write(MvxLogLevel logLevel, string message, Exception? exception = null)
            //{
            //    var formattedMessage = MessageFormatter(mName, logLevel, message, exception);

            //    if (Colors.TryGetValue(logLevel, out var color))
            //    {
            //        var originalColor = Console.ForegroundColor;
            //        try
            //        {
            //            Console.ForegroundColor = color;
            //            Console.WriteLine(formattedMessage);
            //        }
            //        finally
            //        {
            //            Console.ForegroundColor = originalColor;
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(formattedMessage);
            //    }
            //}
        }
    }
}
