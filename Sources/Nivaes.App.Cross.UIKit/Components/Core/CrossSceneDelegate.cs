using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.UIKitLib;

public abstract class CrossSceneDelegate
    : UIResponder, IUIWindowSceneDelegate,
    ICrossLifetime, IPlatformApplication
{
    public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

    private IServiceProvider? _services;

    private ICrossApplication? _application;

    [Export("window")] public UIWindow? Window { get; private set; }

    public IServiceProvider ServiceProvider
    {
        [DebuggerHidden] get => _services!;
    }

    public ICrossApplication Application
    {
        [DebuggerHidden] get => _application!;
    }

    public CrossSceneDelegate()
    {
        IPlatformApplication.Current = this;
    }

    protected abstract CrossApp CreateCrossApp(UIWindow window);

    [Export("scene:willConnectToSession:options:")]
    public virtual void WillConnect(
        UIScene scene,
        UISceneSession session,
        UISceneConnectionOptions connectionOptions)
    {
        if (scene is UIWindowScene windowScene)
        {
            Window = new UIWindow(windowScene);
        }
        else
        {
            Window = new UIWindow();
        }
        //MvxIosSetupSingleton
        //    .EnsureSingletonAvailable(this, Window)
        //    .EnsureInitialized();
        //RunAppStart();

        var crossApp = CreateCrossApp(Window);

        var rootContext = new CrossContext(crossApp.Services);

        var applicationContext = rootContext.MakeApplicationScope(this);

        _services = applicationContext.Services;

        //_services?.InvokeLifecycleEvents<iOSLifecycle.WillFinishLaunching>(del => del(application, launchOptions));

        //InitializeContainer(crossApp.Services);

        _application = _services.GetRequiredService<ICrossApplication>();

        IPlatformApplication.Current!.Application.Setup();
        
        Regiesters();

        IPlatformApplication.Current!.Application.Initialize();

        Window?.MakeKeyAndVisible();

        FireLifetimeChanged(CrossLifetimeEvent.Launching);
    }
    private void Regiesters()
    {
        Parallel.Invoke(
            RegisterServices,
            RegisterConverters,
            RegisterCombiners,
            RegisterPresenterActions,
            RegisterViewsActions
        );
    }

    protected virtual void RegisterServices()
    {
        ServiceProvider
            .TargetBindingFactoryRegistry()
            .BindingNameRegister();
    }

    protected virtual void RegisterConverters()
    {
        UIKitLib.GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected virtual void RegisterCombiners()
    {
        UIKitLib.GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    protected virtual void RegisterPresenterActions()
    {
        UIKitLib.GeneratedPresenterActionsExtensions.RegisterPresenterActions(ServiceProvider);
    }

    protected virtual void RegisterViewsActions()
    {
        UIKitLib.GeneratedViewsExtensions.RegisterViewsActions();
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

    private void FireLifetimeChanged(CrossLifetimeEvent which)
    {
        var handler = LifetimeChanged;
        handler?.Invoke(this, new CrossLifetimeEventArgs(which));
    }
}