namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public abstract class CrossMainThreadDispatchingObject
    {
        protected ICrossMainThreadAsyncDispatcher AsyncDispatcher => IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossMainThreadAsyncDispatcher>();

        protected readonly ILogger Logger;

        public CrossMainThreadDispatchingObject(ILogger logger)
        {
            Logger = logger;
        }

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
            InvokeOnMainThreadAsync(action, maskExceptions);
        }

        protected Task InvokeOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            // this corner case should only happen when there is no IoC
            // i.e. when running in a UnitTest environment, falling back
            // to just executing action
            if (AsyncDispatcher == null)
            {
                try
                {
                    action();
                }
                catch
                {
                    if (!maskExceptions)
                        throw;
                }

                return Task.CompletedTask;
            }

            return AsyncDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}
