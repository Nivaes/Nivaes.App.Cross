﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Logging
{
    using System;

    [Obsolete("Usar AppCenter", true)]
    public interface IMvxLog
    {
        bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception? exception = null, params object[] formatParameters);

        bool IsLogLevelEnabled(MvxLogLevel logLevel);
    }
}
