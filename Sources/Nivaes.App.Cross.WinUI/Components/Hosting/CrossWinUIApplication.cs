using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Nivaes.App.Cross.Hosting;
using Application = Microsoft.UI.Xaml.Application;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace Nivaes.App.Cross.WinUI;

public abstract class CrossWinUIApplication
    : Application, IPlatformApplication
{
    IServiceProvider? _services;

    ICrossApplication? _application;

    internal Frame? RootFrame { get; private set; }

    public Window? MainWindow { get; private set; }

    public IServiceProvider ServiceProvider
    {
        [DebuggerHidden] get => _services!;
    }

    public ICrossApplication Application
    {
        [DebuggerHidden] get => _application!;
    }

    protected abstract CrossApp CreateCrossApp();

    /// <summary>
    /// Invoked when the application is launched normally by the end user.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        //if (_application != null && _services != null)
        //{
        //    _services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));
        //    _services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
        //    return;
        //}

        IPlatformApplication.Current = this;

        var crossApp = CreateCrossApp();

        var rootContext = new CrossContext(crossApp.Services);

        var applicationContext = rootContext.MakeApplicationScope(this);

        _services = applicationContext.Services;

        _services.RegisterWinUICrash(this);

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

        var frame = InitializeFrame();

        //InitializeContainer(crossApp.Services);

        _application = _services.GetRequiredService<ICrossApplication>();

        //this.SetApplicationHandler(_application, applicationContext);

        //this.CreatePlatformWindow(_application, args);

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));

        _application.Setup();
        

        MainWindow!.Activate();

        Regiesters();

        _application.Initialize();
    }

    //protected virtual Window CreateWindow()
    //{
    //    return new Window();
    //}

    protected virtual Frame CreateFrame()
    {
        return new Frame();
    }

    private Frame InitializeFrame()
    {
        //MainWindow ??= new MainWindow(); //CreateWindow();
        MainWindow ??= new Window();

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

    protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new AppException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
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
        WinUI.GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
    }

    protected virtual void RegisterConverters()
    {
        WinUI.GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }

    protected virtual void RegisterCombiners()
    {
        WinUI.GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
    }
    protected virtual void RegisterPresenterActions()
    {
        WinUI.GeneratedPresenterActionsExtensions.RegisterPresenterActions(ServiceProvider);
    }

    protected virtual void RegisterViewsActions()
    {
        WinUI.GeneratedViewsExtensions.RegisterViewsActions();
    }    
}