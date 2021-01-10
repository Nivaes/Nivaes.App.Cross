// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using MvvmCross.Base;
    using MvvmCross.Core;
    using Nivaes.App.Cross.IoC;
    using Autofac;

    public class MvxCommandBase
        : MvxMainThreadDispatchingObject
    {
        private readonly IMvxCommandHelper? mCommandHelper;

        public MvxCommandBase()
        {
            // fallback on MvxWeakCommandHelper if no IoC has been set up
            if (!Mvx.IoCProvider?.TryResolve(out mCommandHelper) ?? true)
                mCommandHelper = new MvxWeakCommandHelper();

            var alwaysOnUIThread = IoCContainer.ComponentContext.Resolve<IMvxSettings>().AlwaysRaiseInpcOnUserInterfaceThread;
            ShouldAlwaysRaiseCECOnUserInterfaceThread = alwaysOnUIThread;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                mCommandHelper.CanExecuteChanged += value;
            }
            remove
            {
                mCommandHelper.CanExecuteChanged -= value;
            }
        }

        public bool ShouldAlwaysRaiseCECOnUserInterfaceThread { get; set; }

        public void RaiseCanExecuteChanged()
        {
            if (ShouldAlwaysRaiseCECOnUserInterfaceThread)
            {
                InvokeOnMainThread(() => mCommandHelper.RaiseCanExecuteChanged(this));
            }
            else
            {
                mCommandHelper.RaiseCanExecuteChanged(this);
            }
        }
    }
}
