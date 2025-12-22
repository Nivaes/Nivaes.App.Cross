namespace MvvmCross.Platforms.Ios.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class MvxSceneDelegate : UIResponder, IUIWindowSceneDelegate, ICrossLifetime
    {
        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

        [Export("window")] public UIWindow? Window { get; set; }

        [Export("scene:willConnectToSession:options:")]
        public virtual void WillConnect(
            UIScene scene,
            UISceneSession session,
            UISceneConnectionOptions connectionOptions)
        {
            if (scene is UIWindowScene windowScene)
            {
                RegisterSetup();
                Window = new UIWindow(windowScene);
                MvxIosSetupSingleton
                    .EnsureSingletonAvailable(this, Window)
                    .EnsureInitialized();
                RunAppStart();
                FireLifetimeChanged(CrossLifetimeEvent.Launching);
            }
        }

        [Export("sceneDidDisconnect:")]
        public virtual void DidDisconnect(UIScene scene)
        {
        }

        [Export("sceneDidBecomeActive:")]
        public virtual void DidBecomeActive(UIScene scene)
        {
            FireLifetimeChanged(CrossLifetimeEvent.ActivatedFromMemory);
        }

        [Export("sceneWillResignActive:")]
        public virtual void WillResignActive(UIScene scene)
        {
            FireLifetimeChanged(CrossLifetimeEvent.Deactivated);
        }

        [Export("sceneWillEnterForeground:")]
        public virtual void WillEnterForeground(UIScene scene)
        {
        }

        [Export("sceneDidEnterBackground:")]
        public virtual void DidEnterBackground(UIScene scene)
        {
        }

        protected virtual void RunAppStart()
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true &&
                startup is { IsStarted: false })
            {
                startup.Start();
            }

            Window?.MakeKeyAndVisible();
        }

        protected abstract void RegisterSetup();

        private void FireLifetimeChanged(CrossLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new CrossLifetimeEventArgs(which));
        }
    }

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class MvxSceneDelegate<TMvxIosSetup, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxSceneDelegate
        where TMvxIosSetup : MvxIosSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxIosSetup>();
        }
    }
}