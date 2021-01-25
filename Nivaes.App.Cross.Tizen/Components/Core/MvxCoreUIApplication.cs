// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    using System;
    using MvvmCross.Core;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxCoreUIApplication<TMvxTizenSetup, TApplication>
        : CoreUIApplication, IMvxLifetime
            where TMvxTizenSetup : MvxTizenSetup<TApplication>, new()
            where TApplication : class, ICrossApplication, new()
    {
        private IMvxTizenSetup Setup { get; }

        public MvxCoreUIApplication()
            : base()
        {
            Setup = new TMvxTizenSetup();
            //RegisterSetup();
        }

        protected override void OnResume()
        {
            base.OnResume();
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        protected override void OnPause()
        {
            base.OnPause();
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        protected override void OnTerminate()
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.OnTerminate();
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            Setup.StartSetupInitialization();
            Setup.PlatformInitialize(this);
            //MvxTizenSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();

            RunAppStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected virtual void RunAppStart()
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                startup.Start();
            }
        }

        protected virtual void RegisterSetup()
        {
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        //protected override void RegisterSetup()
        //{
        //    this.RegisterSetupType<TMvxTizenSetup>();
        //}
    }
}
