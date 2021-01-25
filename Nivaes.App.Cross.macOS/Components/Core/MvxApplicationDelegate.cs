// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.macOS
{
    using System;
    using AppKit;
    using MvvmCross.Core;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.ViewModels;

    public class MvxApplicationDelegate<TMvxMacSetup, TApplication>
        : NSApplicationDelegate, IMvxApplicationDelegate
            where TMvxMacSetup : MvxMacSetup<TApplication>, new()
            where TApplication : class, ICrossApplication, new()
    {
        private IMvxMacSetup Setup { get; }

        private NSWindow window;
        public virtual NSWindow MainWindow
        {
            get
            {
                if (window == null)
                {
                    var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
                    var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
                    window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
                }
                return window;
            }
            set { window = value; }
        }

        public MvxApplicationDelegate() 
        {
            Setup = new TMvxMacSetup();
            RegisterSetup();
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            Setup.PlatformInitialize(this, MainWindow);
            Setup.StartSetupInitialization();
            //MvxMacSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();

            RunAppStart(notification);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
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

        public override void WillBecomeActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidResignActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        //protected override void RegisterSetup()
        //{
        //    this.RegisterSetupType<TMvxMacSetup>();
        //}
    }
}
