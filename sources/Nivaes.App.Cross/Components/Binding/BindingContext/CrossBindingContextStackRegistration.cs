namespace Nivaes.App.Cross
{
    using System;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Logging;

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
            MvxLogHost.Default?.Log(LogLevel.Error,
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
}
