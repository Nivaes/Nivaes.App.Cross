using System;
using Microsoft.Extensions.Logging;
using MvvmCross;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

public class CrossBindingContextStackRegistration<TBindingContext>
    : IDisposable
{
    protected ICrossBindingContextStack<TBindingContext> Stack => Mvx.IoCProvider.Resolve<ICrossBindingContextStack<TBindingContext>>();

    public CrossBindingContextStackRegistration(TBindingContext toRegister)
    {
        Stack.Push(toRegister);
    }

    ~CrossBindingContextStackRegistration()
    {
        CrossLogHost.Default?.Log(LogLevel.Error,
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
