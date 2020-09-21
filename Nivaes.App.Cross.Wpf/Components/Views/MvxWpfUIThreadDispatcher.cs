// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf.Views
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using MvvmCross.Base;

    public class MvxWpfUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override bool IsOnMainThread => _dispatcher.CheckAccess();

        public override async ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                await _dispatcher.InvokeAsync(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            }
        }

        public override async ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            }
            else
            {
                await _dispatcher.InvokeAsync(async () =>
                {
                    if (action != null)
                    {
                        await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
                    }
                }).Task.Unwrap().ConfigureAwait(false);
            }
        }
    }
}
