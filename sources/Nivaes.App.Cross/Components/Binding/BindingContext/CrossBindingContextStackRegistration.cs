using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossBindingContextStackRegistration<TBindingContext>
    : IDisposable
{
    protected ICrossBindingContextStack<TBindingContext> Stack => IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingContextStack<TBindingContext>>();

    public CrossBindingContextStackRegistration(TBindingContext toRegister)
    {
        Stack.Push(toRegister);
    }

    ~CrossBindingContextStackRegistration()
    {
        CrossLoggerHost.Default?.Log(LogLevel.Error,
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
