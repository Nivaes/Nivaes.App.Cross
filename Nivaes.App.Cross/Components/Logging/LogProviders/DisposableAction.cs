﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Logging
{
    using System;

    internal class DisposableAction : IDisposable
    {
        private readonly Action _onDispose;

        public DisposableAction(Action? onDispose = null)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (_onDispose == null) return;
            _onDispose();
        }
    }
}
