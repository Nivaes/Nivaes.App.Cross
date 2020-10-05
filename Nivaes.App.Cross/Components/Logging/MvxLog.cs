﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Logging
{
    using System;
    using MvvmCross.Logging.LogProviders;

    internal class MvxLog
        : IMvxLog
    {
        internal static IMvxLog Instance { get; set; }

        internal const string FailedToGenerateLogMessage = "Failed to generate log message";

        private readonly Logger mLogger;

        internal MvxLog(Logger logger)
        {
            mLogger = logger;
        }

        public bool IsLogLevelEnabled(MvxLogLevel logLevel) => mLogger(logLevel, null);

        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception? exception = null, params object[] formatParameters)
        {
            if (messageFunc == null)
                return mLogger(logLevel, null);

            if (exception is null) throw new ArgumentNullException(nameof(exception));
            if (formatParameters is null) throw new ArgumentNullException(nameof(formatParameters));

            string wrappedMessageFunc()
            {
                try
                {
                    return messageFunc();
                }
                catch (Exception ex)
                {
                    Log(MvxLogLevel.Error, () => FailedToGenerateLogMessage, ex);
                }

                return string.Empty;
            }

            return mLogger(logLevel, wrappedMessageFunc, exception, formatParameters);
        }
    }
}
