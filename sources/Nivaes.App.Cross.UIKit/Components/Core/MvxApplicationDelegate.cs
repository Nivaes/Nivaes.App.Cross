namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;
    using MvvmCross.Platforms.Ios.Core;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit;

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class MvxApplicationDelegate 
        : UIApplicationDelegate, IMvxApplicationDelegate
    {
        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

        public virtual UIWindow? MainWindow { get; set; }

        protected MvxApplicationDelegate()
        {
            RegisterSetup();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            FireLifetimeChanged(CrossLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            FireLifetimeChanged(CrossLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(UIApplication application)
        {
            FireLifetimeChanged(CrossLifetimeEvent.Closing);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
        {
            MainWindow ??= new UIWindow(UIScreen.MainScreen.Bounds);

            MvxIosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();

            RunAppStart(launchOptions);

            FireLifetimeChanged(CrossLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart(object? hint = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true && startup is { IsStarted: false })
            {
                startup.Start(GetAppStartHint(hint));
            }

            MainWindow?.MakeKeyAndVisible();
        }

        protected virtual object? GetAppStartHint(object? hint = null)
        {
            return hint;
        }

        protected abstract void RegisterSetup();

        private void FireLifetimeChanged(CrossLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new CrossLifetimeEventArgs(which));
        }
    }

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class MvxApplicationDelegate<TMvxIosSetup, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxApplicationDelegate
        where TMvxIosSetup : MvxIosSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxIosSetup>();
        }
    }
}