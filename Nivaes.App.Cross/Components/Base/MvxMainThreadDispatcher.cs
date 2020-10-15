// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Base
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross.Logging;

    public abstract class MvxMainThreadDispatcher
        : IMvxMainThreadDispatcher
    {
        public static MvxMainThreadDispatcher? Instance { get; protected set; }

        protected MvxMainThreadDispatcher()
        {
            Instance = this;
        }

        public abstract ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true);

        public abstract ValueTask<bool> ExecuteOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }

        protected internal static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            try
            {
                action();
            }
            catch (Exception ex) when (!TraceException(ex, maskExceptions))
            {
            }
        }

        protected internal static ValueTask<bool> ExceptionMaskedActionAsync(Func<ValueTask<bool>> action, bool maskExceptions)
        {
            if (action == null) throw new NullReferenceException(nameof(action));

            try
            {
                return action();
            }
            catch (Exception ex) when (!TraceException(ex, maskExceptions))
            {
            }

            return new ValueTask<bool>(false);
        }

        private static bool TraceException(Exception exception, bool maskExceptions)
        {
            if (exception is TargetInvocationException targetInvocationException)
            {
                MvxLog.Instance?.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                {
                    MvxLog.Instance?.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MvxLog.Instance?.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                {
                    MvxLog.Instance?.Warn("Exception masked " + exception.ToLongString());
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
