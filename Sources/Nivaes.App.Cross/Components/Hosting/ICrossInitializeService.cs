namespace Nivaes.App.Cross.Hosting;

public interface ICrossInitializeService
{
    void Initialize(IServiceProvider services);
}

public interface ICrossInitializeScopedService
{
    void Initialize(IServiceProvider services);
}
