// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using System.Threading.Tasks;

    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher AsyncDispatcher => (IMvxMainThreadDispatcher)MvxMainThreadDispatcher.Instance!;

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            if (AsyncDispatcher == null)
            {
                try
                {
                    action();
                }
                catch when (maskExceptions)
                {
                }

                return;
            }

            var _ = AsyncDispatcher.ExecuteOnMainThread(action, maskExceptions).AsTask();
        }

        protected ValueTask InvokeOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            if (AsyncDispatcher == null)
            {
                try
                {
                    return action();
                }
                catch when (maskExceptions)
                {
                }

                return new ValueTask();
            }

            return AsyncDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}
