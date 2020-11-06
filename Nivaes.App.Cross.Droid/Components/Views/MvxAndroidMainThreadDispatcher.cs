// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MvvmCross.Base;

    public class MvxAndroidMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public override ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }

            return new ValueTask<bool>(true);
        }

        public override ValueTask<bool> ExecuteOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                return ExceptionMaskedActionAsync(action, maskExceptions);
            }
            else
            {
                Application.SynchronizationContext.Post(async _ =>
                {
                    _ = await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(true);
                }, null);

                return new ValueTask<bool>(true);
            }
        }
    }
}
