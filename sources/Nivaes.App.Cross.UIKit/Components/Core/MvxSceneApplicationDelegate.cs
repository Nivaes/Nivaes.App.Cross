namespace MvvmCross.Platforms.Ios.Core
{
    using Nivaes.App.Cross;

    public abstract class MvxSceneApplicationDelegate
        : UIApplicationDelegate, ICrossLifetime
    {
        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
        public virtual string SceneConfigurationName { get; } = "MvxSceneConfiguration";
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            return true;
        }

        public override UISceneConfiguration GetConfiguration(UIApplication application,
            UISceneSession connectingSceneSession, UISceneConnectionOptions options) =>
            new(SceneConfigurationName, connectingSceneSession.Role);
    }
}