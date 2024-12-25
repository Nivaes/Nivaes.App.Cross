using System.Diagnostics;

namespace Nivaes.App.Cross
{
    public abstract class CrossApplication : ICrossApplication, IDisposable
    {
        private ICrossApplicationStart? mApplicationStart;

        public ICrossApplicationStart ApplicationStart { get => mApplicationStart!; }

        public CrossApplication(ICrossApplicationStart applicationStart)
        {
            mApplicationStart = applicationStart;
        }

        //private IMvxViewModelLocator? _defaultLocator;

        //private IMvxViewModelLocator DefaultLocator
        //{
        //    get
        //    {
        //        _defaultLocator ??= CreateDefaultViewModelLocator();
        //        return _defaultLocator;
        //    }
        //}

        //protected virtual IMvxViewModelLocator CreateDefaultViewModelLocator()
        //{
        //    return new MvxDefaultViewModelLocator();
        //}

        //public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        //{
        //    // do nothing
        //}

        /// <summary>
        /// Any initialization steps that can be done in the background
        /// </summary>
        public virtual void Initialize()
        {
            // do nothing
        }

        ///// <summary>
        ///// Any initialization steps that need to be done on the UI thread
        ///// </summary>
        //public virtual Task Startup()
        //{
        //    MvxLogHost.Default?.Log(LogLevel.Trace, "AppStart: Application Startup - On UI thread");
        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// If the application is restarted (eg primary activity on Android 
        ///// can be restarted) this method will be called before Startup
        ///// is called again
        ///// </summary>
        //public virtual void Reset()
        //{
        //    // do nothing
        //}

        //public IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        //{
        //    return DefaultLocator;
        //}

        //protected void RegisterCustomAppStart<TMvxAppStart>()
        //    where TMvxAppStart : class, IMvxAppStart
        //{
        //    Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, TMvxAppStart>();
        //}

        //protected void RegisterAppStart<TViewModel>()
        //    where TViewModel : IMvxViewModel
        //{
        //    Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel>>();
        //}

        //protected void RegisterAppStart(IMvxAppStart appStart)
        //{
        //    Mvx.IoCProvider?.RegisterSingleton(appStart);
        //}

        //protected virtual void RegisterAppStart<TViewModel, TParameter>()
        //  where TViewModel : IMvxViewModel<TParameter> where TParameter : class
        //{
        //    Mvx.IoCProvider?.ConstructAndRegisterSingleton<IMvxAppStart, MvxAppStart<TViewModel, TParameter>>();
        //}

        //protected IEnumerable<Type> CreatableTypes()
        //{
        //    return CreatableTypes(GetType().GetTypeInfo().Assembly);
        //}

        //protected IEnumerable<Type> CreatableTypes(Assembly assembly)
        //{
        //    return assembly.CreatableTypes();
        //}

        public void Dispose()
        {
            if (Debugger.IsAttached)
                Debugger.Break();
            else
                Debugger.Launch();
        }
    }
}
