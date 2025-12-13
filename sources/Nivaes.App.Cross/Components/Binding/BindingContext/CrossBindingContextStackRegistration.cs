namespace Nivaes.App.Cross
{
    using System;
    using Microsoft.Extensions.Logging;

    public class CrossBindingContextStackRegistration<TBindingContext>
        : IDisposable
    {
        //protected ICrossBindingContextStack<TBindingContext> Stack => Cross.IoCProvider.Resolve<ICrossBindingContextStack<TBindingContext>>();

        public CrossBindingContextStackRegistration(TBindingContext toRegister)
        {
            throw new NotImplementedException();
            //Stack.Push(toRegister);
        }

        ~CrossBindingContextStackRegistration()
        {
            CrossLogHost.Default?.Log(LogLevel.Error,
                "You should always Dispose of CrossBindingContextStackRegistration");
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
                throw new NotImplementedException();
                //Stack.Pop();
            }
        }
    }
}
