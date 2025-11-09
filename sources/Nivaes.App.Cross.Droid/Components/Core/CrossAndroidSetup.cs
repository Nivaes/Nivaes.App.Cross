namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Android.Content;
    using Android.Views;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Android.Presenters;

    [Obsolete("No es AoT compatible")]
    public abstract class CrossAndroidSetup
        : CrossSetup, ICrossAndroidGlobals, ICrossAndroidSetup
    {
        private CrossCurrentTopActivity? _currentTopActivity;
        private ICrossAndroidViewPresenter? _presenter;

        public void PlatformInitialize(Application application)
        {
            ArgumentNullException.ThrowIfNull(application);

            ApplicationContext = application;

            if (_currentTopActivity != null)
                return;

            _currentTopActivity = new CrossCurrentTopActivity();
            application.RegisterActivityLifecycleCallbacks(_currentTopActivity);
        }

        public virtual Assembly ExecutableAssembly => ViewAssemblies.FirstOrDefault() ?? GetType().Assembly;

        public Context? ApplicationContext { get; private set; }

        protected override void InitializeFirstChance(ICrossIoCProvider iocProvider)
        {
            throw new NotImplementedException();
            //ValidateArguments(iocProvider);

            //InitializeLifetimeMonitor(iocProvider);
            //InitializeAndroidCurrentTopActivity(iocProvider);
            //RegisterPresenter(iocProvider);

            //iocProvider.RegisterSingleton<ICrossAndroidGlobals>(this);

            //var intentResultRouter = new CrossIntentResultSink();
            //iocProvider.RegisterSingleton<ICrossIntentResultSink>(intentResultRouter);
            //iocProvider.RegisterSingleton<ICrossIntentResultSource>(intentResultRouter);

            //var viewModelTemporaryCache = new CrossSingleViewModelCache();
            //iocProvider.RegisterSingleton<ICrossSingleViewModelCache>(viewModelTemporaryCache);

            //var viewModelMultiTemporaryCache = new CrossMultipleViewModelCache();
            //iocProvider.RegisterSingleton<CrossMultipleViewModelCache>(viewModelMultiTemporaryCache);
            //base.InitializeFirstChance(iocProvider);
        }

        protected virtual void InitializeAndroidCurrentTopActivity(ICrossIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var currentTopActivity = CreateAndroidCurrentTopActivity();
            iocProvider.RegisterSingleton(currentTopActivity);
        }

        protected virtual ICrossAndroidCurrentTopActivity CreateAndroidCurrentTopActivity()
        {
            if (_currentTopActivity == null)
                throw new InvalidOperationException($"Please call {nameof(PlatformInitialize)} first");

            return _currentTopActivity;
        }

        protected virtual void InitializeLifetimeMonitor(ICrossIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var lifetimeMonitor = CreateLifetimeMonitor();

            iocProvider.RegisterSingleton<ICrossAndroidActivityLifeTimeListener>(lifetimeMonitor);
            iocProvider.RegisterSingleton<ICrossLifetime>(lifetimeMonitor);
        }

        protected virtual CrossAndroidLifeTimeMonitor CreateLifetimeMonitor()
        {
            return new CrossAndroidLifeTimeMonitor();
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected virtual void InitializeSavedStateConverter(ICrossIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var converter = CreateSavedStateConverter();
            iocProvider.RegisterSingleton(converter);
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected virtual ICrossSavedStateConverter CreateSavedStateConverter()
        {
            return new CrossSavedStateConverter();
        }

        //protected override ICrossViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    if (ApplicationContext == null)
        //        throw new InvalidOperationException("Cannot create Views Container without ApplicationContext");

        //    var container = CreateViewsContainer(ApplicationContext);
        //    iocProvider.RegisterSingleton<ICrossAndroidViewModelRequestTranslator>(container);
        //    iocProvider.RegisterSingleton<ICrossAndroidViewModelLoader>(container);
        //    if (container is not CrossViewsContainer viewsContainer)
        //        throw new CrossException("CreateViewsContainer must return an MvxViewsContainer");
        //    return viewsContainer;
        //}

        protected virtual ICrossAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            return new CrossAndroidViewsContainer(applicationContext);
        }

        //protected ICrossAndroidViewPresenter Presenter
        //{
        //    get
        //    {
        //        _presenter ??= CreateViewPresenter();
        //        return _presenter;
        //    }
        //}

        //protected virtual ICrossAndroidViewPresenter CreateViewPresenter()
        //{
        //    return new CrossAndroidViewPresenter(AndroidViewAssemblies);
        //}

        //protected override ICrossViewDispatcher CreateViewDispatcher()
        //{
        //    return new CrossAndroidViewDispatcher(Presenter);
        //}

        //protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    var presenter = Presenter;
        //    iocProvider.RegisterSingleton(presenter);
        //    iocProvider.RegisterSingleton<ICrossViewPresenter>(presenter);
        //}

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected override void InitializeLastChance(ICrossIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            InitializeSavedStateConverter(iocProvider);
            base.InitializeLastChance(iocProvider);
        }

        //[RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        //protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    var bindingBuilder = CreateBindingBuilder();
        //    bindingBuilder.DoRegistration(iocProvider);
        //}

        //[RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        //protected virtual CrossBindingBuilder CreateBindingBuilder()
        //{
        //    return new CrossAndroidBindingBuilder(FillValueConverters, FillValueCombiners, FillTargetFactories,
        //        FillBindingNames, FillViewTypes, FillAxmlViewTypeResolver, FillNamespaceListViewTypeResolver);
        //}

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void FillViewTypes(IMvxTypeCache cache)
        {
            ArgumentNullException.ThrowIfNull(cache);

            foreach (var assembly in AndroidViewAssemblies)
            {
                cache.AddAssembly(assembly);
            }
        }

        //protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        //{
        //    // this base class does nothing
        //}

        //protected virtual void FillAxmlViewTypeResolver(IMvxAxmlNameViewTypeResolver viewTypeResolver)
        //{
        //    ArgumentNullException.ThrowIfNull(viewTypeResolver);

        //    foreach (var kvp in ViewNamespaceAbbreviations)
        //    {
        //        viewTypeResolver.ViewNamespaceAbbreviations[kvp.Key] = kvp.Value;
        //    }
        //}

        //protected virtual void FillNamespaceListViewTypeResolver(IMvxNamespaceListViewTypeResolver viewTypeResolver)
        //{
        //    ArgumentNullException.ThrowIfNull(viewTypeResolver);

        //    foreach (var viewNamespace in ViewNamespaces)
        //    {
        //        viewTypeResolver.Add(viewNamespace);
        //    }
        //}

        //[RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        //protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        //{
        //    ArgumentNullException.ThrowIfNull(registry);

        //    registry.Fill(ValueConverterAssemblies);
        //    registry.Fill(ValueConverterHolders);
        //}

        //protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        //{
        //    // this base class does nothing
        //}

        protected virtual IEnumerable<Type> ValueConverterHolders => new List<Type>();

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
        typeof(CrossDatePicker).Assembly,
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

    public abstract class MvxAndroidSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : CrossAndroidSetup
        where TApplication : class, ICrossApplication, new()
    {
        //protected override ICrossApplication CreateApp(IMvxIoCProvider iocProvider) =>
        //    iocProvider.IoCConstruct<TApplication>();

        //[RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        //public override IEnumerable<Assembly> GetViewModelAssemblies()
        //{
        //    return [typeof(TApplication).GetTypeInfo().Assembly];
        //}
    }
}