namespace Nivaes.App.Cross.UIKitLib;

public abstract class CrossAppDelegate
    : UIApplicationDelegate, IMvxApplicationDelegate
{

    public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

    public virtual string SceneConfigurationName { get; } = "CrossSceneConfiguration";

    public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        return true;
    }

    public override UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
    {
        return new UISceneConfiguration(SceneConfigurationName, connectingSceneSession.Role);
    }
}