﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System;
    using Windows.UI.Core;
    using MvvmCross.Base;
    using System.Threading.Tasks;

    public class MvxWindowsMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly CoreDispatcher mUiDispatcher;

        public MvxWindowsMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            mUiDispatcher = uiDispatcher ?? throw new ArgumentNullException(nameof(uiDispatcher));
        }

        public override bool IsOnMainThread => mUiDispatcher.HasThreadAccess;

        public override async ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
                return;
            }

            await mUiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ExceptionMaskedAction(action, maskExceptions);
            });
        }

        public override async ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            }

            await mUiDispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            });
        }
    }
}
