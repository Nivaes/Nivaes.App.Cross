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

public abstract class CrossApplication 
    : Application, IPlatformApplication
{
    IServiceProvider? _services;

    IApplication? _application;

    //IServiceProvider IPlatformApplication.Services => _services!;

    internal Frame? RootFrame { get; set; }
    internal Window? MainWindow { get; private set; }

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

    //protected CrossWinUIApplication()
    //{
    //    //RegisterSetup();
    //}

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

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

        var frame = InitializeFrame();

        InitializeContainer(crossApp.Services);

        _application = _services.GetRequiredService<IApplication>();
        var navigationService = _services.GetRequiredService<ICrossNavigationService>();

        //this.SetApplicationHandler(_application, applicationContext);

        //this.CreatePlatformWindow(_application, args);

        //_services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));

        var initializeViewModelType = _application.Initialize();

        MainWindow!.Activate();

        await initializeViewModelType.NavigateToFirstViewModel(navigationService);
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

    protected virtual Window CreateWindow()
    {
        return new Window();
    }

    protected virtual Frame CreateFrame()
    {
        return new Frame();
    }

    private Frame InitializeFrame()
    {
        MainWindow ??= CreateWindow();

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
        // ToDo: Integrar con log.
        throw new CrossException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
    }

    // ToDO: Buscar donde registar ICrossSuspensionManager.
    private void InitializeContainer(IServiceProvider serviceProvider)
    {
        var suspensionManager = new CrossSuspensionManager();
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.Merge(new WinUISubcontainer());

        container.AddInstance<ICrossSuspensionManager>(suspensionManager);

        //if (_suspensionManagerSessionStateKey != null)
        //    suspensionManager.RegisterFrame(RootFrame, _suspensionManagerSessionStateKey);

        container.AddInstance<ICrossWindowsViewModelLoader>(new CrossWindowsViewsContainer(_services!));
        container.AddInstance<IServiceProvider>(serviceProvider);



        //container.AddInstance<ICrossViewModelByNameLookup> (new CrossViewModelByNameLookup());
    }
}