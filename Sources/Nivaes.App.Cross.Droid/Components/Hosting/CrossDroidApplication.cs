using System.Diagnostics;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Droid
{
    public abstract class CrossDroidApplication
        : Application, IPlatformApplication
    {
        IServiceProvider? _services;

        ICrossApplication? _application;

        public static CrossDroidApplication Current { [DebuggerHidden] get; [DebuggerHidden] private set; } = null!;

        public IServiceProvider ServiceProvider { [DebuggerHidden] get => _services!; }

        public ICrossApplication Application { [DebuggerHidden] get => _application!; }

        protected CrossDroidApplication(IntPtr handle, JniHandleOwnership ownership) 
            : base(handle, ownership)
        {
            Current = this;
            IPlatformApplication.Current = this;
        }

        protected abstract CrossApp CreateCrossApp();

        public override async void OnCreate()
        {
            //RegisterActivityLifecycleCallbacks(new ActivityLifecycleCallbacks());

            var crossApp = CreateCrossApp();

            var rootContext = new CrossAndroidContext(crossApp.Services, this);

            var applicationContext = rootContext.MakeApplicationScope(this);

            _services = applicationContext.Services;

            //_services.InvokeLifecycleEvents<AndroidLifecycle.OnApplicationCreating>(del => del(this));

            //InitializeContainer(crossApp.Services);

            _application = _services.GetRequiredService<ICrossApplication>();

            var currentTopActivity = _services.GetRequiredService<IMvxAndroidCurrentTopActivity>();
            base.RegisterActivityLifecycleCallbacks(currentTopActivity);

            Regiesters();

            //this.SetApplicationHandler(_application, applicationContext);

            //_services?.InvokeLifecycleEvents<AndroidLifecycle.OnApplicationCreate>(del => del(this));

            //var initializeViewModelType = _application.Initialize();

            base.OnCreate();

            //var navigationService = _services.GetRequiredService<ICrossNavigationService>();

            //await initializeViewModelType.NavigateToFirstViewModel(navigationService);
        }

        private void Regiesters()
        {
            RegisterServices();
            RegisterConverters();
            RegisterCombiners();
            RegisterPresenterActions();
            RegisterViewsActions();
        }

        protected virtual void RegisterServices()
        {
            ServiceProvider
                .TargetBindingFactoryRegistry()
                .BindingNameRegister();
        }

        protected virtual void RegisterConverters()
        {
            Droid.GeneratedConverterExtensions.RegisterConverters(ServiceProvider);
        }

        protected virtual void RegisterCombiners()
        {
            Droid.GeneratedCombinerExtensions.RegisterCombiners(ServiceProvider);
        }

        protected virtual void RegisterPresenterActions()
        {
            Droid.GeneratedPresenterActionsExtensions.RegisterPresenterActions(ServiceProvider);
        }

        protected virtual void RegisterViewsActions()
        {
            Droid.GeneratedViewsExtensions.RegisterViewsActions();
        }

        public override void OnLowMemory()
        {
            //_services?.InvokeLifecycleEvents<AndroidLifecycle.OnApplicationLowMemory>(del => del(this));

            base.OnLowMemory();
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            //_services?.InvokeLifecycleEvents<AndroidLifecycle.OnApplicationTrimMemory>(del => del(this, level));

            base.OnTrimMemory(level);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            //_services?.InvokeLifecycleEvents<AndroidLifecycle.OnApplicationConfigurationChanged>(del => del(this, newConfig));

            base.OnConfigurationChanged(newConfig);
        }

        //IServiceProvider IPlatformApplication.Services => _services!;

        //IApplication IPlatformApplication.Application => _application!;


        //public class ActivityLifecycleCallbacks : Java.Lang.Object, IActivityLifecycleCallbacks
        //{
        //public void OnActivityCreated(Activity activity, Bundle? savedInstanceState) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnCreate>(del => del(activity, savedInstanceState));

        //public void OnActivityStarted(Activity activity) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnStart>(del => del(activity));

        //public void OnActivityResumed(Activity activity) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnResume>(del => del(activity));

        //public void OnActivityPaused(Activity activity) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnPause>(del => del(activity));

        //public void OnActivityStopped(Activity activity) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnStop>(del => del(activity));

        //public void OnActivitySaveInstanceState(Activity activity, Bundle outState) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnSaveInstanceState>(del => del(activity, outState));

        //public void OnActivityDestroyed(Activity activity) =>
        //    IPlatformApplication.Current?.Services?.InvokeLifecycleEvents<AndroidLifecycle.OnDestroy>(del => del(activity));
        //}


        //// ToDO: Buscar donde registar ICrossSuspensionManager.
        //private void InitializeContainer(IServiceProvider serviceProvider)
        //{
        //    //var suspensionManager = new CrossSuspensionManager();
        //    var container = Singleton<CrossIoCServiceContainer>.Instance;
        //    container.Merge(new DroidIoCServiceContainer());

        //    //container.AddInstance<ICrossSuspensionManager>(suspensionManager);

        //    //if (_suspensionManagerSessionStateKey != null)
        //    //    suspensionManager.RegisterFrame(RootFrame, _suspensionManagerSessionStateKey);

        //    //container.AddInstance<ICrossWindowsViewModelLoader>(new CrossWindowsViewsContainer(_services!));
        //    container.AddInstance<IServiceProvider>(serviceProvider);



        //    //container.AddInstance<ICrossViewModelByNameLookup> (new CrossViewModelByNameLookup());
        //}
    }
}
