namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class CrossSceneDelegate : UIResponder, IUIWindowSceneDelegate, ICrossLifetime
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
                throw new NotImplementedException();
                //RegisterSetup();
                //Window = new UIWindow(windowScene);
                //MvxIosSetupSingleton
                //    .EnsureSingletonAvailable(this, Window)
                //    .EnsureInitialized();
                //RunAppStart();
                //FireLifetimeChanged(MvxLifetimeEvent.Launching);
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
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true &&
            //    startup is { IsStarted: false })
            //{
            //    startup.Start();
            //}

            //Window?.MakeKeyAndVisible();
        }

        protected abstract void RegisterSetup();

        private void FireLifetimeChanged(CrossLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new CrossLifetimeEventArgs(which));
        }
    }

    [RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
    public abstract class MvxSceneDelegate<TMvxIosSetup, TApplication> : CrossSceneDelegate
        where TMvxIosSetup : CrossIosSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxIosSetup>();
        }
    }
}