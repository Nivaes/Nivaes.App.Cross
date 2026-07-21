using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossMainThreadDispatchingObject
    {
        private static Lazy<ICrossMainThreadDispatcher> _mainThreadDispatcher = 
            new Lazy<ICrossMainThreadDispatcher>(() => IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossMainThreadDispatcher>());

        protected ICrossMainThreadDispatcher MainThreadDispatcher => _mainThreadDispatcher.Value;

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
            return MainThreadDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}
