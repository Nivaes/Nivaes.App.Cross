namespace Nivaes.App.Cross.UIKit
{
    public abstract class CrossSceneApplicationDelegate : UIApplicationDelegate, ICrossLifetime
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