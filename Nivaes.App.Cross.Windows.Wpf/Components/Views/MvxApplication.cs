// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf.Views
{
    using System.Windows;
    using MvvmCross.Core;
    using MvvmCross.Platforms.Wpf.Core;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross;

    public abstract class MvxApplication : Application
    {
        public MvxApplication() : base()
        {
            RegisterSetup();
        }

        public virtual void ApplicationInitialized()
        {
            if (MainWindow == null) return;

            MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow).EnsureInitialized();

            RunAppStart();
        }

        protected virtual void RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                startup.Start(GetAppStartHint(hint));
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public class MvxApplication<TMvxWpfSetup, TApplication> : MvxApplication
       where TMvxWpfSetup : MvxWpfSetup<TApplication>, new()
       where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxWpfSetup>();
        }
    }
}
