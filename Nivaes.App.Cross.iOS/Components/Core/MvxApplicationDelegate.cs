﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.iOS
{
    using System;
    using Foundation;
    using MvvmCross.Core;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    public abstract class MvxApplicationDelegate<TMvxIosSetup, TApplication>
        : UIApplicationDelegate, IMvxApplicationDelegate
           where TMvxIosSetup : MvxIosSetup<TApplication>, new()
           where TApplication : class, ICrossApplication, new()
    {
        public IMvxIosSetup Setup { get; }

        /// <summary>
        /// UIApplicationDelegate.Window doesn't really exist / work. It was added by Xamarin.iOS templates 
        /// </summary>
        public new virtual UIWindow? Window { get; set; }

        public MvxApplicationDelegate() : base()
        {
            Setup = new TMvxIosSetup();

            RegisterSetup();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            if (Window == null)
                Window = new UIWindow(UIScreen.MainScreen.Bounds);

            //_ = MvxIosSetupSingleton.EnsureSingletonAvailable(this, Window).EnsureInitialized();
            _ = Setup.StartSetupInitialization();

            RunAppStart(launchOptions);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart(object? hint = null)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                _ = startup.Start(GetAppStartHint(hint)).AsTask();
            }

            Window!.MakeKeyAndVisible();
        }

        protected virtual object? GetAppStartHint(object? hint = null)
        {
            return hint;
        }

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<MvxLifetimeEventArgs>? LifetimeChanged;

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }
    }


        //protected override void RegisterSetup()
        //{
        //    this.RegisterSetupType<TMvxIosSetup>();
        //}
}
