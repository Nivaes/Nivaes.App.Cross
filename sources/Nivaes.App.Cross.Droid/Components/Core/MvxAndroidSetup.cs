using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Android.Content;
using Android.Views;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Core;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid
{
    public abstract class MvxAndroidSetup
        : CrossSetup, IMvxAndroidGlobals, IMvxAndroidSetup
    {
        private MvxCurrentTopActivity? _currentTopActivity;
        private IMvxAndroidViewPresenter? _presenter;

        public void PlatformInitialize(Application application)
        {
            ArgumentNullException.ThrowIfNull(application);

            ApplicationContext = application;

            if (_currentTopActivity != null)
                return;

            _currentTopActivity = new MvxCurrentTopActivity();
            application.RegisterActivityLifecycleCallbacks(_currentTopActivity);
        }

        public virtual Assembly ExecutableAssembly => ViewAssemblies.FirstOrDefault() ?? GetType().Assembly;

        public Context? ApplicationContext { get; private set; }

        protected override void InitializeFirstChance()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            InitializeLifetimeMonitor(container);
            InitializeAndroidCurrentTopActivity(container);
            RegisterPresenter(container);

            container.AddInstance<IMvxAndroidGlobals>(this);

            var intentResultRouter = new MvxIntentResultSink();
            container.AddInstance<IMvxIntentResultSink>(intentResultRouter);
            container.AddInstance<IMvxIntentResultSource>(intentResultRouter);

            var viewModelTemporaryCache = new MvxSingleViewModelCache();
            container.AddInstance<IMvxSingleViewModelCache>(viewModelTemporaryCache);

            var viewModelMultiTemporaryCache = new MvxMultipleViewModelCache();
            container.AddInstance<IMvxMultipleViewModelCache>(viewModelMultiTemporaryCache);
            base.InitializeFirstChance();
        }

        protected virtual void InitializeAndroidCurrentTopActivity(CrossIoCServiceContainer container)
        {
            var currentTopActivity = CreateAndroidCurrentTopActivity();
            container.AddInstance(currentTopActivity);
        }

        protected virtual IMvxAndroidCurrentTopActivity CreateAndroidCurrentTopActivity()
        {
            if (_currentTopActivity == null)
                throw new InvalidOperationException($"Please call {nameof(PlatformInitialize)} first");

            return _currentTopActivity;
        }

        protected virtual void InitializeLifetimeMonitor(IoCServiceContainer container)
        {
            var lifetimeMonitor = CreateLifetimeMonitor();

            container.AddInstance<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            container.AddInstance<ICrossLifetime>(lifetimeMonitor);
            //iocProvider.RegisterSingleton<IMvxAndroidActivityLifetimeListener>(lifetimeMonitor);
            //iocProvider.RegisterSingleton<ICrossLifetime>(lifetimeMonitor);
        }

        protected virtual MvxAndroidLifetimeMonitor CreateLifetimeMonitor()
        {
            return new MvxAndroidLifetimeMonitor();
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected virtual void InitializeSavedStateConverter(IMvxIoCProvider iocProvider)
        {
            var converter = CreateSavedStateConverter();
            iocProvider.RegisterSingleton(converter);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected virtual IMvxSavedStateConverter CreateSavedStateConverter()
        {
            return new MvxSavedStateConverter();
        }

        protected override ICrossViewsContainer CreateViewsContainer()
        {
            if (ApplicationContext == null)
                throw new InvalidOperationException("Cannot create Views Container without ApplicationContext");

            var container = CreateViewsContainer(ApplicationContext);
            throw new NotImplementedException("Carga de Views");
            //iocProvider.RegisterSingleton<IMvxAndroidViewModelRequestTranslator>(container);
            //iocProvider.RegisterSingleton<IMvxAndroidViewModelLoader>(container);
            if (container is not CrossViewsContainer viewsContainer)
                throw new CrossException("CreateViewsContainer must return an MvxViewsContainer");
            return viewsContainer;
        }

        protected virtual IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new MvxAndroidViewsContainer(applicationContext);
        }

        protected IMvxAndroidViewPresenter Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            return new MvxAndroidViewDispatcher(Presenter);
        }

        protected virtual void RegisterPresenter(CrossIoCServiceContainer container)
        {
            var presenter = Presenter;

            container.AddInstance(presenter);
            container.AddInstance<ICrossViewPresenter>(presenter);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected override void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            InitializeSavedStateConverter(iocProvider);
            base.InitializeLastChance(iocProvider);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        [Obsolete("No usar reflection")]
        protected virtual CrossBindingBuilder CreateBindingBuilder()
        {
            return new MvxAndroidBindingBuilder(FillValueConverters, FillValueCombiners, FillTargetFactories,
                FillBindingNames, FillViewTypes, FillAxmlViewTypeResolver, FillNamespaceListViewTypeResolver);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void FillViewTypes(IMvxTypeCache cache)
        {
            ArgumentNullException.ThrowIfNull(cache);

            foreach (var assembly in AndroidViewAssemblies)
            {
                cache.AddAssembly(assembly);
            }
        }

        protected virtual void FillBindingNames(ICrossBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
        {
            ArgumentNullException.ThrowIfNull(viewTypeResolver);

            foreach (var kvp in ViewNamespaceAbbreviations)
            {
                viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
            }
        }

        protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
        {
            ArgumentNullException.ThrowIfNull(viewTypeResolver);

            foreach (var viewNamespace in ViewNamespaces)
            {
                viewTypeResolver.Add(viewNamespace);
            }
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            ArgumentNullException.ThrowIfNull(registry);

            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual IEnumerable<Type> ValueConverterHolders => new List<Type>();

        [Obsolete("No usar reflection")]
        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual IDictionary<string, string> ViewNamespaceAbbreviations => new Dictionary<string, string>
    {
        { "Mvx", "mvvmcross.platforms.android.binding.views" }
    };

        protected virtual IEnumerable<string> ViewNamespaces => new List<string>
    {
        "Android.Views",
        "Android.Widget",
        "Android.Webkit",
        "MvvmCross.Platforms.Android.Views",
        "MvvmCross.Platforms.Android.Binding.Views"
    };

        protected virtual IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>()
    {
        typeof(View).Assembly,
        typeof(MvxDatePicker).Assembly,
        GetType().Assembly,
    };

        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }

        protected override ICrossNameMapping CreateViewToViewModelNaming()
        {
            return new CrossPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment");
        }
    }

    public abstract class MvxAndroidSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication>
        : MvxAndroidSetup
            where TApplication : class, ICrossApplication, new()
    {
        protected override void CreateApp()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddDelegate<ICrossApplication>(container =>
            {
                return new TApplication();
            });
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return [typeof(TApplication).GetTypeInfo().Assembly];
        }
    }
}