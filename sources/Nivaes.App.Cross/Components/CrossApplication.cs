namespace Nivaes.App.Cross
{
    using System.Diagnostics;
    using OpenTelemetry;
    using OpenTelemetry.Metrics;

    public abstract class CrossApplication 
        : ICrossApplication, IDisposable
    {
        private readonly ICrossApplicationStart? mApplicationStart;

        public ICrossApplicationStart ApplicationStart { get => mApplicationStart!; }

        protected CrossApplication(ICrossApplicationStart applicationStart)
        {
            mApplicationStart = applicationStart;

            var meterProvider = Sdk
                .CreateMeterProviderBuilder()
                .AddMeter("Cross")
                //.AddPrometheusHttpListener(options => options.UriPrefixes = new string[] { "http://localhost:8464/" })
                //.AddPrometheusHttpListener(options => options.UriPrefixes = new string[] { "http://10.0.2.2:9464/" }) // for android
                .Build();
        }

        //private ICrossViewModelLocator? _defaultLocator;

        //private ICrossViewModelLocator DefaultLocator
        //{
        //    get
        //    {
        //        _defaultLocator ??= CreateDefaultViewModelLocator();
        //        return _defaultLocator;
        //    }
        //}

        //protected virtual ICrossViewModelLocator CreateDefaultViewModelLocator()
        //{
        //    return new CrossDefaultViewModelLocator();
        //}

        //public virtual void LoadPlugins(ICrossPluginManager pluginManager)
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
        //    CrossLogHost.Default?.Log(LogLevel.Trace, "AppStart: Application Startup - On UI thread");
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

        //public ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request)
        //{
        //    return DefaultLocator;
        //}

        //protected void RegisterCustomAppStart<TCrossAppStart>()
        //    where TCrossAppStart : class, ICrossAppStart
        //{
        //    Cross.IoCProvider?.ConstructAndRegisterSingleton<ICrossAppStart, TCrossAppStart>();
        //}

        //protected void RegisterAppStart<TViewModel>()
        //    where TViewModel : ICrossViewModel
        //{
        //    Cross.IoCProvider?.ConstructAndRegisterSingleton<ICrossAppStart, CrossAppStart<TViewModel>>();
        //}

        //protected void RegisterAppStart(ICrossAppStart appStart)
        //{
        //    Cross.IoCProvider?.RegisterSingleton(appStart);
        //}

        //protected virtual void RegisterAppStart<TViewModel, TParameter>()
        //  where TViewModel : ICrossViewModel<TParameter> where TParameter : class
        //{
        //    Cross.IoCProvider?.ConstructAndRegisterSingleton<ICrossAppStart, CrossAppStart<TViewModel, TParameter>>();
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
