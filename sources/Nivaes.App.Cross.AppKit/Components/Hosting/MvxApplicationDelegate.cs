using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.AppKitOS;
using Nivaes.App.Cross.Hosting;
using Nivaes.IoC;

namespace Nivaes.App.Cross.AppKitOS;

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class MvxApplicationDelegate : 
    NSApplicationDelegate, IMvxApplicationDelegate, IPlatformApplication
{
    private IServiceProvider? _services;

    private IApplication? _application;

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

        //var rootContext = new MauiContext(mauiApp.Services);

        //_applicationContext = rootContext.MakeApplicationScope(this);

        //_services = _applicationContext.Services;

        //_services?.InvokeLifecycleEvents<iOSLifecycle.WillFinishLaunching>(del => del(application, launchOptions));

        _services
                .TargetBindingFactoryRegistry()
                .BindingNameRegister();
    }

    [Obsolete]
    protected virtual void RunAppStart(object hint = null)
    {
        var startup = IPlatformApplication.Current!.Services.GetRequiredService<ICrossAppStart>();

        //if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true && !startup.IsStarted)
        if(!startup.IsStarted)
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
