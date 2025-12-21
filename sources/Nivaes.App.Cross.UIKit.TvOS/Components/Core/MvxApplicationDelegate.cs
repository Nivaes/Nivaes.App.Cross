namespace MvvmCross.Platforms.Tvos.Core
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Core;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    [RequiresUnreferencedCode("RegisterSetup may register types that are not preserved by default in the application")]
    public abstract class MvxApplicationDelegate 
        : UIApplicationDelegate, IMvxApplicationDelegate
    {
        /// <summary>
        /// UIApplicationDelegate.Window doesn't really exist / work. It was added by Xamarin.iOS templates 
        /// </summary>
        public virtual UIWindow MainWindow { get; set; }

        protected MvxApplicationDelegate()
        {
            RegisterSetup();
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            MainWindow ??= new UIWindow(UIScreen.MainScreen.Bounds);

            MvxTvosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();
            RunAppStart(launchOptions);

            FireLifetimeChanged(CrossLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true && !startup.IsStarted)
            {
                startup.Start(GetAppStartHint(hint));
            }
            MainWindow.MakeKeyAndVisible();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
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

        private void FireLifetimeChanged(CrossLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new CrossLifetimeEventArgs(which));
        }

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<CrossLifetimeEventArgs> LifetimeChanged;
    }

    [RequiresUnreferencedCode("RegisterSetup may register types that are not preserved by default in the application")]
    public abstract class MvxApplicationDelegate<TMvxTvosSetup, TApplication> 
        : MvxApplicationDelegate
           where TMvxTvosSetup : MvxTvosSetup<TApplication>, new()
           where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxTvosSetup>();
        }
    }
}
