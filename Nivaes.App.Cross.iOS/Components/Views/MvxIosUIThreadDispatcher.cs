// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using UIKit;

    public abstract class MvxIosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext mUISynchronizationContext;

        protected MvxIosUIThreadDispatcher()
        {
            mUISynchronizationContext = SynchronizationContext.Current;
            if (mUISynchronizationContext == null)
                throw new MvxException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
        }

        public override ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
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
                UIApplication.SharedApplication.BeginInvokeOnMainThread(async () =>
                {
                    await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
                });

                return new ValueTask<bool>(true);
            }
        }

        public override bool IsOnMainThread => mUISynchronizationContext == SynchronizationContext.Current;
    }
}
