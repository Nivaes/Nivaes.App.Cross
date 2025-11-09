namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class CrossApplicationDelegate 
        : UIApplicationDelegate, ICrossApplicationDelegate
    {
        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

        public virtual UIWindow? MainWindow { get; set; }

        protected CrossApplicationDelegate()
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

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            throw new NotImplementedException();
            //MainWindow ??= new UIWindow(UIScreen.MainScreen.Bounds);

            //MvxIosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();

            //RunAppStart(launchOptions);

            //FireLifetimeChanged(MvxLifetimeEvent.Launching);
            //return true;
        }

        protected virtual void RunAppStart(object? hint = null)
        {
            throw new NotImplementedException();

            //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true && startup is { IsStarted: false })
            //{
            //    startup.Start(GetAppStartHint(hint));
            //}

            //MainWindow?.MakeKeyAndVisible();
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
    public abstract class CrossApplicationDelegate<TCrossIosSetup, TApplication> : CrossApplicationDelegate
        where TCrossIosSetup : CrossIosSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TCrossIosSetup>();
        }
    }
}