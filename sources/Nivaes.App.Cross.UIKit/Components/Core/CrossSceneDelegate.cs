using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.IoC;

namespace Nivaes.App.Cross.UIKitOS;

[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
public abstract class CrossSceneDelegate
    : UIResponder, IUIWindowSceneDelegate,
    ICrossLifetime, IPlatformApplication
{
    public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

    private IServiceProvider? _services;

    private IApplication? _application;

    [Export("window")] public UIWindow? Window { get; private set; }

    public IServiceProvider Services
    {
        get => _services!;
        protected set => _services = value;
    }

    public IApplication Application
    {
        get => _application!;
        protected set => _application = value;
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

        _application = _services.GetRequiredService<IApplication>();

        var initializeViewModelType = IPlatformApplication.Current!.Application.Initialize();
        var navigationService = IPlatformApplication.Current!.Services.GetRequiredService<ICrossNavigationService>();

        initializeViewModelType.NavigateToFirstViewModel(navigationService).GetAwaiter().GetResult();

        FireLifetimeChanged(CrossLifetimeEvent.Launching);
    }

    //// ToDO: Buscar donde registar ICrossSuspensionManager.
    //private void InitializeContainer(IServiceProvider serviceProvider)
    //{
    //    //var suspensionManager = new CrossSuspensionManager();
    //    var container = Singleton<CrossIoCServiceContainer>.Instance;
    //    container.Merge(new UIKitSubcontainer());

    //    //container.AddInstance<ICrossSuspensionManager>(suspensionManager);

    //    //if (_suspensionManagerSessionStateKey != null)
    //    //    suspensionManager.RegisterFrame(RootFrame, _suspensionManagerSessionStateKey);

    //    //container.AddInstance<ICrossWindowsViewModelLoader>(new CrossWindowsViewsContainer(_services!));
    //    container.AddInstance<IServiceProvider>(serviceProvider);



    //    //container.AddInstance<ICrossViewModelByNameLookup> (new CrossViewModelByNameLookup());
    //}

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

    //protected virtual void RunAppStart()
    //{
    //    //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true &&
    //    //    startup is { IsStarted: false })
    //    //{
    //        //startup.Start();
    //    //}

    //    Window?.MakeKeyAndVisible();
    //}

    //protected abstract void RegisterSetup();

    private void FireLifetimeChanged(CrossLifetimeEvent which)
    {
        var handler = LifetimeChanged;
        handler?.Invoke(this, new CrossLifetimeEventArgs(which));
    }
}

//[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
//public abstract class MvxSceneDelegate<TMvxIosSetup, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxSceneDelegate
//    where TMvxIosSetup : MvxIosSetup<TApplication>, new()
//    where TApplication : class, ICrossApplication, new()
//{
//    protected override void RegisterSetup()
//    {
//        this.RegisterSetupType<TMvxIosSetup>();
//    }
//}