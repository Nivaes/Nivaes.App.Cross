using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Nivaes.App.Cross.Hosting;
using Nivaes.IoC;
using Windows.UI;
using Application = Microsoft.UI.Xaml.Application;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Nivaes.App.Cross.WinUI;

public abstract class CrossWinUIApplication 
    : Application
{
    IServiceProvider? _services;

    IApplication? _application;

    //IServiceProvider IPlatformApplication.Services => _services!;

    internal Frame? RootFrame { get; set; }
    internal Window? MainWindow { get; private set; }

    //protected CrossWinUIApplication()
    //{
    //    //RegisterSetup();
    //}

    protected abstract CrossApp CreateCrossApp();

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        //if (_application != null && _services != null)
        //{
        //    _services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));
        //    _services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
        //    return;
        //}

        //IPlatformApplication.Current = this;
        var mauiApp = CreateCrossApp();

        var rootContext = new CrossContext(mauiApp.Services);

        //var applicationContext = rootContext.MakeApplicationScope(this);

        //_services = applicationContext.Services;

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

        //_application = _services.GetRequiredService<IApplication>();

        //this.SetApplicationHandler(_application, applicationContext);

        //this.CreatePlatformWindow(_application, args);

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));

        MainWindow = new Window();
        MainWindow.Activate();
    }

    //protected virtual void RunAppStart(string arguments)
    //{
    //    var instance = CrossWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, arguments, "Suspend");

    //    if (RootFrame.Content == null)
    //    {
    //        instance.EnsureInitialized();

    //        if (Mvx.IoCProvider.TryResolve(out ICrossAppStart? startup) && !(startup?.IsStarted ?? false))
    //        {
    //            startup?.Start(GetAppStartHint(arguments));
    //        }
    //    }
    //}

    //protected virtual object? GetAppStartHint(object? hint = null)
    //{
    //    return hint;
    //}

    protected virtual Window CreateWindow()
    {
        return new Window();
    }

    protected virtual Frame InitializeFrame(string arguments)
    {
        if (MainWindow == null)
        {
            MainWindow = CreateWindow();
        }

        var rootFrame = MainWindow.Content as Frame;

        if (rootFrame == null)
        {
            rootFrame = CreateFrame();
            rootFrame.NavigationFailed += OnNavigationFailed;

            MainWindow.Content = rootFrame;
        }

        RootFrame = rootFrame;

        return rootFrame;
    }

    protected virtual Frame CreateFrame()
    {
        return new Frame();
    }

    protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new CrossException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
    }

    //protected virtual void RegisterSetup()
    //{

    //}
}

//public class CrossApplication<TMvxWinUiSetup, TApplication> : CrossWinUIApplication
//   where TMvxWinUiSetup : MvxWindowsSetup<TApplication>, new()
//   where TApplication : class, ICrossApp, new()
//{
//    protected override void RegisterSetup()
//    {
//        this.RegisterSetupType<TMvxWinUiSetup>();
//    }
//}
