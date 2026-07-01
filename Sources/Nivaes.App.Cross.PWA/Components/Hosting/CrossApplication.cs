using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Web;

public abstract class CrossApplication
    : /*Application,*/ IPlatformApplication
{
    IServiceProvider? _services;

    IApplication? _application;

    IServiceProvider IPlatformApplication.Services => _services!;

    //internal Frame? RootFrame { get; set; }
    //internal Window? MainWindow { get; private set; }

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

    protected abstract CrossApp CreateCrossApp();

}