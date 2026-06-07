using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;
using Nivaes.IoC;

namespace Nivaes.App.Cross.UIKitOS;

[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
public abstract class CrossAppDelegate 
    : UIApplicationDelegate, IMvxApplicationDelegate //, IPlatformApplication
{
    //private IServiceProvider? _services;

    //private IApplication? _application;

    public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;

    public virtual UIWindow? MainWindow { get; set; }

    //public IServiceProvider Services
    //{
    //    get => _services!;
    //    protected set => _services = value;
    //}

    //public IApplication Application
    //{
    //    get => _application!;
    //    protected set => _application = value;
    //}

    //protected CrossAppDelegate()
    //{
    //    IPlatformApplication.Current = this;
    //}

    //protected abstract CrossApp CreateCrossApp();

    //public override bool WillFinishLaunching(UIApplication application, NSDictionary? launchOptions)
    //{
    //    var crossApp = CreateCrossApp();

    //    var rootContext = new CrossContext(crossApp.Services);

    //    var applicationContext = rootContext.MakeApplicationScope(this);

    //    _services = applicationContext.Services;

    //    //_services?.InvokeLifecycleEvents<iOSLifecycle.WillFinishLaunching>(del => del(application, launchOptions));

    //    InitializeContainer(crossApp.Services);

    //    _application = _services.GetRequiredService<IApplication>();
    //    //var navigationService = _services.GetRequiredService<ICrossNavigationService>();

    //    //var initializeViewModelType = _application.Initialize();

    //    //return base.WillFinishLaunching(application, launchOptions);

    //    //Task.Run(async () => await initializeViewModelType.NavigateToFirstViewModel(navigationService));
    //    //initializeViewModelType.NavigateToFirstViewModel(navigationService).GetAwaiter().GetResult();

    //    return true;
    //}
    public override bool WillFinishLaunching(UIApplication application, NSDictionary? launchOptions)
    {
        return true;
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

        //MvxIosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();

        RunAppStart(launchOptions);

        FireLifetimeChanged(CrossLifetimeEvent.Launching);
        return true;
    }

    protected virtual void RunAppStart(object? hint = null)
    {
        //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart? startup) == true && startup is { IsStarted: false })
        //{
        //    startup.Start(GetAppStartHint(hint));
        //}

        MainWindow?.MakeKeyAndVisible();
    }

    protected virtual object? GetAppStartHint(object? hint = null)
    {
        return hint;
    }

    //protected abstract void RegisterSetup();

    private void FireLifetimeChanged(CrossLifetimeEvent which)
    {
        var handler = LifetimeChanged;
        handler?.Invoke(this, new CrossLifetimeEventArgs(which));
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
}

//[RequiresUnreferencedCode("This class uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
//public abstract class MvxApplicationDelegate<TMvxIosSetup, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : CrossAppDelegate
//    where TMvxIosSetup : MvxIosSetup<TApplication>, new()
//    where TApplication : class, ICrossApplication, new()
//{
//    protected override void RegisterSetup()
//    {
//        this.RegisterSetupType<TMvxIosSetup>();
//    }
//}