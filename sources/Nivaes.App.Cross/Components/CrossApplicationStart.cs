using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public abstract class CrossApplicationStart : ICrossApplicationStart, IDisposable
    {
        //protected readonly INavigationService NavigationService;
        //protected readonly ICrossApplication Application;

        //private int startHasCommenced;

        //protected CrossApplicationStart(ICrossApplication application, INavigationService navigationService)
        //{
        //    Application = application;
        //    NavigationService = navigationService;
        //}

        //public void Start(object? hint = null)
        //{
        //    StartAsync(hint).GetAwaiter().GetResult();
        //}

        //public async Task StartAsync(object? hint = null)
        //{
        //    // Check whether Start has commenced, and return if it has
        //    //if (Interlocked.CompareExchange(ref startHasCommenced, 1, 0) == 1)
        //    //    return;

        //    //var applicationHint = await ApplicationStartup(hint);
        //    //if (applicationHint != null)
        //    //{
        //    //    MvxLogHost.Default?.Log(LogLevel.Trace, "Hint ignored in default MvxAppStart");
        //    //}

        //    //await NavigateToFirstViewModel(applicationHint);
        //    await NavigateToFirstViewModel(hint);
        //}

        public abstract Task NavigateToFirstViewModel(object? hint = null);

        //protected virtual async Task NavigateToFirstViewModel(object? hint = null)
        //{
        //    try
        //    {
        //        //await NavigationService.Navigate<TViewModel>();
        //        await NavigationService.Navigate<ViewModel>();
        //    }
        //    catch (System.Exception exception)
        //    {
        //        //throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(TViewModel).Name);
        //        throw exception.Wrap("Problem navigating to ViewModel {0}", typeof(ViewModel).Name);
        //    }
        //}

        //protected virtual async Task<object?> ApplicationStartup(object? hint = null)
        //{
        //    await Application.Startup();
        //    return hint;
        //}

        //public virtual bool IsStarted => startHasCommenced != 0;

        //public virtual void ResetStart()
        //{
        //    Reset();
        //    Interlocked.Exchange(ref startHasCommenced, 0);
        //}

        //protected virtual void Reset()
        //{
        //    Application.Reset();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            else
                Debugger.Launch();
        }
    }
}
