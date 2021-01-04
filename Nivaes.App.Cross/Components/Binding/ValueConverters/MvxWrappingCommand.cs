// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.WeakSubscription;
using Nivaes.App.Cross.Logging;

namespace MvvmCross.Binding.ValueConverters
{
    public class MvxWrappingCommand
        : ICommand
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged");

        private readonly ICommand mWrapped;
        private readonly object mCommandParameterOverride;
        private readonly IDisposable mCanChangedEventSubscription;

        public MvxWrappingCommand(ICommand wrapped, object commandParameterOverride)
        {
            mWrapped = wrapped;
            mCommandParameterOverride = commandParameterOverride;

            if (mWrapped != null)
            {
                mCanChangedEventSubscription = CanExecuteChangedEventInfo.WeakSubscribe(mWrapped, WrappedOnCanExecuteChanged);
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void WrappedOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            CanExecuteChanged?.Invoke(this, eventArgs);
        }

        public bool CanExecute(object? parameter = null)
        {
            if (mWrapped == null)
                return false;

            //if (parameter != null)
            //    MvxLog.Instance?.Warn("Non-null parameter will be ignored in MvxWrappingCommand.CanExecute");

            return mWrapped.CanExecute(mCommandParameterOverride);
        }

        public void Execute(object? parameter = null)
        {
            if (mWrapped == null)
                return;

            //if (parameter != null)
            //    MvxLog.Instance?.Warn("Non-null parameter overridden in MvxWrappingCommand");

            mWrapped.Execute(mCommandParameterOverride);
        }

        public event EventHandler CanExecuteChanged;
    }
}
