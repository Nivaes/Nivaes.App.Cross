// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf.Views
{
    using System.Windows;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Wpf;

    public class MvxApplication<TMvxWpfSetup, TApplication>
        : Application, IMvxApplication
            where TMvxWpfSetup : MvxWpfSetup<TApplication>, new()
            where TApplication : class, ICrossApplication, new() 
    {
        private IMvxWpfSetup Setup { get; }

        public MvxApplication() 
        {
            Setup = new TMvxWpfSetup();
            //RegisterSetup();
        }

        public virtual void ApplicationInitialized()
        {
            if (MainWindow == null) return;

            Setup.PlatformInitialize(Dispatcher, MainWindow);
            Setup.StartSetupInitialization();

            //MvxWpfSetupSingleton.EnsureSingletonAvailable(Dispatcher, MainWindow).EnsureInitialized();

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

        //protected override void RegisterSetup()
        //{
        //    this.RegisterSetupType<TMvxWpfSetup>();
        //}
    }
}
