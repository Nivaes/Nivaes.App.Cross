﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using System.Threading.Tasks;

    public interface IMvxMainThreadDispatcher
    {
        ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true);
        ValueTask<bool> ExecuteOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
