// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Base;

    public class CallbackMockMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Func<Action, bool> mCallback;

        public CallbackMockMainThreadDispatcher(Func<Action, bool> callback)
        {
            mCallback = callback;
        }

        public override bool IsOnMainThread => true;

        public override ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            mCallback(() =>
            {
                ExceptionMaskedAction(action, maskExceptions);
            });

            return new ValueTask();
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            mCallback(async () =>
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            });

            return new ValueTask();
        }
    }
}
