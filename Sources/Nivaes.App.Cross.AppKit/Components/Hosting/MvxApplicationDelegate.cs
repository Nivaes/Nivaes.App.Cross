using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.AppKitOS;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.AppKitLib;

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class MvxApplicationDelegate :
    NSApplicationDelegate, IMvxApplicationDelegate, IPlatformApplication
{
    private IServiceProvider? _services;

    private ICrossApplication? _application;

    public IServiceProvider ServiceProvider
    {
        [DebuggerHidden] get => _services!;
    }

    public ICrossApplication Application
    {
        [DebuggerHidden] get => _application!;
    }

    protected MvxApplicationDelegate()
        : base()
    {
        RegisterSetup();

        IPlatformApplication.Current = this;
    }

    protected abstract CrossApp CreateCrossApp();

    public override void DidFinishLaunching(Foundation.NSNotification notification)
    {
        var crossApp = CreateCrossApp();

        //MvxMacSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
        //RunAppStart(notification);

        //FireLifetimeChanged(CrossLifetimeEvent.Launching);

        var rootContext = new CrossContext(crossApp.Services);

        var applicationContext = rootContext.MakeApplicationScope(this);

        _services = applicationContext.Services;

        _application = _services.GetRequiredService<ICrossApplication>();
        IPlatformApplication.Current!.Application.Setup();
        var initializeViewModelType = IPlatformApplication.Current!.Application.Initialize();

        RegisterServices();
        RegisterConverters();
        RegisterCombiners();
        RegisterPresenterActions();

        var navigationService = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossNavigationService>();
        initializeViewModelType.NavigateToFirstViewModel(navigationService).GetAwaiter().GetResult();

        //_services?.InvokeLifecycleEvents<iOSLifecycle.WillFinishLaunching>(del => del(application, launchOptions));
    }

    protected virtual void RegisterServices()
    {
        ServiceProvider
            .TargetBindingFactoryRegistry()
            .BindingNameRegister();
    }

    protected virtual void RegisterConverters()
    {
        AppKitLib.GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected virtual void RegisterCombiners()
    {
        AppKitLib.GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    protected virtual void RegisterPresenterActions()
    {
        AppKitLib.GeneratedPresenterActionsExtensions.RegisterPresenterActions(ServiceProvider);
    }

    [Obsolete]
    protected virtual void RunAppStart(object hint = null)
    {
        var startup = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossAppStart>();

        //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true && !startup.IsStarted)
        if (!startup.IsStarted)
        {
            startup.Start(GetAppStartHint(hint));
        }
    }

    protected virtual object? GetAppStartHint(object? hint = null)
    {
        return hint;
    }

    public override void WillBecomeActive(Foundation.NSNotification notification)
    {
        FireLifetimeChanged(CrossLifetimeEvent.ActivatedFromMemory);
    }

    public override void DidResignActive(Foundation.NSNotification notification)
    {
        FireLifetimeChanged(CrossLifetimeEvent.Deactivated);
    }

    public override void WillTerminate(Foundation.NSNotification notification)
    {
        FireLifetimeChanged(CrossLifetimeEvent.Closing);
    }

    private void FireLifetimeChanged(CrossLifetimeEvent which)
    {
        LifetimeChanged?.Invoke(this, new CrossLifetimeEventArgs(which));
    }

    protected virtual void RegisterSetup()
    {
    }

    public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
}

//[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
//public class MvxApplicationDelegate<TMvxMacSetup, TApplication> : MvxApplicationDelegate
//    where TMvxMacSetup : MvxMacSetup<TApplication>, new()
//    where TApplication : class, ICrossApplication, new()
//{
//    protected override void RegisterSetup()
//    {
//        this.RegisterSetupType<TMvxMacSetup>();
//    }
//}
