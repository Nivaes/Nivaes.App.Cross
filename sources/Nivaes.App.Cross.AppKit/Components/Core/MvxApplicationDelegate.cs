namespace MvvmCross.Platforms.Mac.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppKit;
    using Nivaes.App.Cross;

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        protected MvxApplicationDelegate() 
            : base()
        {
            RegisterSetup();
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            MvxMacSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
            RunAppStart(notification);

            FireLifetimeChanged(CrossLifetimeEvent.Launching);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true && !startup.IsStarted)
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
            FireLifetimeChanged(CrossLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidResignActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(CrossLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(CrossLifetimeEvent.Closing);
        }

        private void FireLifetimeChanged(CrossLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new CrossLifetimeEventArgs(which));
        }

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<CrossLifetimeEventArgs> LifetimeChanged;
    }

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public class MvxApplicationDelegate<TMvxMacSetup, TApplication> : MvxApplicationDelegate
        where TMvxMacSetup : MvxMacSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxMacSetup>();
        }
    }
}
