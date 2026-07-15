using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

public class CrossBindingContextStackRegistration<TBindingContext>
    : IDisposable
{
    protected ICrossBindingContextStack<TBindingContext> Stack => IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossBindingContextStack<TBindingContext>>();

    public CrossBindingContextStackRegistration(TBindingContext toRegister)
    {
        Stack.Push(toRegister);
    }

    ~CrossBindingContextStackRegistration()
    {
        CrossLoggerHost.GetLogger<CrossBindingContextStackRegistration<TBindingContext>>().Log(LogLevel.Error,
            "You should always Dispose of MvxBindingContextStackRegistration");
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stack.Pop();
        }
    }
}
