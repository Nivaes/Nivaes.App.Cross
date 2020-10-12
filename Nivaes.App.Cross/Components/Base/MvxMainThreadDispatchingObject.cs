// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using System.Threading.Tasks;

    public abstract class MvxMainThreadDispatchingObject
    {
        protected static IMvxMainThreadDispatcher AsyncDispatcher => (IMvxMainThreadDispatcher)MvxMainThreadDispatcher.Instance!;

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            var asyncDispatcher = AsyncDispatcher;

            if (asyncDispatcher == null)
            {
                try
                {
                    action();
                }
                catch when (maskExceptions)
                {
                }
            }
            else
            {
                _ = asyncDispatcher.ExecuteOnMainThread(action, maskExceptions).AsTask();
            }
        }

        protected ValueTask<bool> InvokeOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            var asyncDispatcher = AsyncDispatcher;

            if (asyncDispatcher == null)
            {
                try
                {
                    return action();
                }
                catch when (maskExceptions)
                {
                }

                return new ValueTask<bool>(false);
            }
            else
            {
                return asyncDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
            }
        }
    }
}
