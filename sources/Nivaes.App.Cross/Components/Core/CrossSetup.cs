namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Core;
    using MvvmCross.IoC;
    using MvvmCross.Logging;
    using MvvmCross.Plugin;
    using MvvmCross.ViewModels.Result;

    public abstract class CrossSetup 
        : ICrossSetup
    {
        public event EventHandler<CrossSetupStateEventArgs>? StateChanged;

        private static readonly object Lock = new();
        private CrossSetupState _state;
        private IMvxIoCProvider? _iocProvider;

        protected static Action<IMvxIoCProvider>? RegisterSetupDependencies { get; set; }

        protected static Func<ICrossSetup>? SetupCreator { get; set; }

        protected static List<Assembly> ViewAssemblies { get; } = [];

        protected ILogger? SetupLog { get; private set; }

        public CrossSetupState State
        {
            get => _state;
            private set
            {
                _state = value;
                FireStateChange(value);
            }
        }

        public static void RegisterSetupType<TMvxSetup>(params Assembly[] assemblies) where TMvxSetup : CrossSetup, new()
        {
            // We are using double-checked locking here to avoid overhead of locking if the
            // SetupCreator is already created
            if (SetupCreator is null)
            {
                lock (Lock)
                {
                    if (SetupCreator is null)
                    {
                        ViewAssemblies.AddRange(assemblies);
                        if (ViewAssemblies.Count == 0)
                        {
                            // fall back to all assemblies. Assembly.GetEntryAssembly() always returns
                            // null on Xamarin platforms do not use it!
                            ViewAssemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
                        }

                        // Avoid creating the instance of Setup right now, instead
                        // take a reference to the type in a way that we can avoid
                        // using reflection to create the instance.
                        SetupCreator = () => new TMvxSetup();

                        return;
                    }
                }
            }

            MvxLogHost.Default?.LogInformation("Setup: RegisterSetupType already called");
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public static ICrossSetup? Instance()
        {
            var instance = SetupCreator?.Invoke() ?? CrossSetupExtensions.CreateSetup<CrossSetup>();
            return instance;
        }

        protected abstract ICrossApplication CreateApp(IMvxIoCProvider iocProvider);

        protected abstract ICrossViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider);

        protected abstract ICrossViewDispatcher CreateViewDispatcher();

        public virtual void InitializePrimary()
        {
            if (State != CrossSetupState.Uninitialized)
            {
                SetupLog?.Log(LogLevel.Trace,
                    "InitializePrimary() called when State is not Uninitialized. State: {State}", State);
                return;
            }

            try
            {
                State = CrossSetupState.InitializingPrimary;
                _iocProvider = InitializeIoC();

                InitializeLoggingServices(_iocProvider);

                // Register the default setup dependencies before
                // invoking the static call back.
                // Developers can either extend the MvxSetup and override
                // the RegisterDefaultSetupDependencies method, or can provide a
                // callback method by setting the RegisterSetupDependencies method
                RegisterDefaultSetupDependencies(_iocProvider);
                RegisterSetupDependencies?.Invoke(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Primary start");
                SetupLog?.Log(LogLevel.Trace, "Setup: FirstChance start");
                InitializeFirstChance(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: MvvmCross settings start");
                InitializeSettings(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Singleton Cache start");
                InitializeSingletonCache();
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewDispatcher start");
                InitializeViewDispatcher(_iocProvider);
                State = CrossSetupState.InitializedPrimary;
            }
            catch (Exception e)
            {
                SetupLog?.Log(LogLevel.Error, e, "InitializePrimary() Failed initializing primary dependencies");
                throw;
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public virtual void InitializeSecondary()
        {
            if (State != CrossSetupState.InitializedPrimary)
            {
                SetupLog?.Log(LogLevel.Trace,
                    "InitializeSecondary() called when State is not InitializedPrimary. State: {State}", State);
                return;
            }

            if (_iocProvider == null)
            {
                SetupLog?.Log(LogLevel.Error, "InitializeSecondary() IoC Provider is null");
                throw new InvalidOperationException("Cannot continue setup with null IoCProvider");
            }

            try
            {
                State = CrossSetupState.InitializingSecondary;
                SetupLog?.Log(LogLevel.Trace, "Setup: Bootstrap actions");
                PerformBootstrapActions();
                SetupLog?.Log(LogLevel.Trace, "Setup: StringToTypeParser start");
                InitializeStringToTypeParser(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: FillableStringToTypeParser start");
                InitializeFillableStringToTypeParser(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Create App");
                var app = InitializeMvxApplication(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: NavigationService");
                InitializeNavigationService(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: ResultViewModelManager");
                InitializeResultViewModelManager(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
                InitializeViewModelTypeFinder(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
                InitializeViewsContainer(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
                var lookup = InitializeLookupDictionary(_iocProvider);
                if (lookup != null)
                {
                    SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                    InitializeViewLookup(lookup, _iocProvider);
                }
                else
                {
                    SetupLog?.LogWarning("Lookup dictionary is null returning from {MethodName}",
                        nameof(InitializeLookupDictionary));
                }

                SetupLog?.Log(LogLevel.Trace, "Setup: CommandCollectionBuilder start");
                InitializeCommandCollectionBuilder(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: NavigationSerializer start");
                InitializeNavigationSerializer(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                InitializeInpcInterception(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                InitializeViewModelCache(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: BindingBuilder start");
                InitializeBindingBuilder(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: PluginManagerFramework start");
                var pluginManager = InitializePluginFramework(_iocProvider);
                if (pluginManager != null)
                {
                    app?.LoadPlugins(pluginManager);
                    SetupLog?.Log(LogLevel.Trace, "Setup: App start");
                }
                else
                {
                    SetupLog?.LogWarning("PluginManager was null returning from {MethodName}",
                        nameof(InitializePluginFramework));
                }

                if (app != null)
                {
                    InitializeApp(app);
                }
                else
                {
                    SetupLog?.LogWarning("App instance is null returning from {MethodName}",
                        nameof(InitializeMvxApplication));
                }

                SetupLog?.Log(LogLevel.Trace, "Setup: LastChance start");
                InitializeLastChance(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Secondary end");
                State = CrossSetupState.Initialized;
            }
            catch (Exception e)
            {
                SetupLog?.Log(LogLevel.Error, e, "InitializeSecondary() failed initializing secondary dependencies");
                throw;
            }
        }

        protected virtual void InitializeSingletonCache()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            CrossSingletonCache.Initialize();
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        protected virtual void InitializeInpcInterception(IMvxIoCProvider iocProvider)
        {
            // by default no Inpc calls are intercepted
        }

        protected virtual ICrossChildViewModelCache? InitializeViewModelCache(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var cache = CreateViewModelCache(iocProvider);
            return cache;
        }

        protected virtual ICrossChildViewModelCache? CreateViewModelCache(IMvxIoCProvider iocProvider)
        {
            return iocProvider.Resolve<ICrossChildViewModelCache>();
        }

        protected virtual ICrossSettings? InitializeSettings(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var settings = CreateSettings(iocProvider);
            return settings;
        }

        protected virtual ICrossSettings? CreateSettings(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossSettings>();
        }

        protected virtual ICrossStringToTypeParser? InitializeStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateStringToTypeParser(iocProvider);
        }

        protected virtual ICrossStringToTypeParser? CreateStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossStringToTypeParser>();
        }

        protected virtual ICrossFillableStringToTypeParser? InitializeFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var parser = CreateFillableStringToTypeParser(iocProvider);
            if (parser != null)
                iocProvider.RegisterSingleton(parser);

            return parser;
        }

        protected virtual ICrossFillableStringToTypeParser? CreateFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossStringToTypeParser>() as ICrossFillableStringToTypeParser;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual void PerformBootstrapActions()
        {
            var bootstrapRunner = new CrossBootstrapRunner();
            foreach (var assembly in GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        protected virtual ICrossNavigationSerializer? InitializeNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateNavigationSerializer(iocProvider);
        }

        protected virtual ICrossNavigationSerializer? CreateNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossNavigationSerializer>();
        }

        protected virtual ICrossCommandCollectionBuilder? InitializeCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateCommandCollectionBuilder(iocProvider);
        }

        protected virtual ICrossCommandCollectionBuilder? CreateCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossCommandCollectionBuilder>();
        }

        protected virtual IMvxIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            iocProvider.RegisterSingleton(iocProvider);
            iocProvider.RegisterSingleton<ICrossSetup>(this);
            return iocProvider;
        }

        protected virtual void RegisterDefaultSetupDependencies(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.LazyConstructAndRegisterSingleton<ICrossSettings, CrossSettings>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossStringToTypeParser, CrossStringToTypeParser>();
            iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(iocProvider, GetPluginConfiguration));
            iocProvider.RegisterSingleton(CreateApp(iocProvider));
            iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelLoader, CrossViewModelLoader>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossNavigationService, ICrossViewModelLoader, ICrossViewDispatcher, IMvxIoCProvider>(
                (loader, dispatcher, p) => new CrossNavigationService(loader, dispatcher, p));
            iocProvider.LazyConstructAndRegisterSingleton<IMvxResultViewModelManager, MvxResultViewModelManager>();
            iocProvider.RegisterSingleton(() => new CrossViewModelByNameLookup());
            iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelByNameLookup, CrossViewModelByNameLookup>(
                nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelByNameRegistry, CrossViewModelByNameLookup>(
                nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelTypeFinder, CrossViewModelViewTypeFinder>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossTypeToTypeLookupBuilder, CrossViewModelViewLookupBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossCommandCollectionBuilder, CrossCommandCollectionBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossNavigationSerializer, CrossStringDictionaryNavigationSerializer>();
            iocProvider.LazyConstructAndRegisterSingleton<ICrossChildViewModelCache, CrossChildViewModelCache>();

            iocProvider.RegisterType<ICrossCommandHelper, CrossWeakCommandHelper>();
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxIoCProvider.Initialize(CreateIocOptions());
        }

        protected virtual void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            // always the very first thing to get initialized - after IoC and base platform
            // base class implementation is empty by default
        }

        protected virtual void InitializeLoggingServices(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var logProvider = CreateLogProvider();
            var loggerFactory = CreateLogFactory();

            if (logProvider != null)
            {
                iocProvider.RegisterSingleton(logProvider);
                loggerFactory?.AddProvider(logProvider);
            }

            if (loggerFactory != null)
            {
                iocProvider.RegisterSingleton(loggerFactory);
                iocProvider.RegisterType(typeof(ILogger<>), typeof(Logger<>));
                SetupLog = loggerFactory.CreateLogger<CrossSetup>();
            }
        }

        protected abstract ILoggerProvider? CreateLogProvider();
        protected abstract ILoggerFactory? CreateLogFactory();

        protected virtual ICrossViewModelLoader? CreateViewModelLoader(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossViewModelLoader>();
        }

        protected virtual ICrossNavigationService? CreateNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossNavigationService>();
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual IMvxPluginManager? InitializePluginFramework(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var pluginManager = CreatePluginManager(iocProvider);
            if (pluginManager != null)
                LoadPlugins(pluginManager);
            return pluginManager;
        }

        protected virtual IMvxPluginManager? CreatePluginManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxPluginManager>();
        }

        protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public virtual IEnumerable<Assembly> GetPluginAssemblies()
        {
            var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies
                .AsParallel()
                .Where(assembly => AssemblyReferencesMvvmCross(assembly, mvvmCrossAssemblyName));
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        private static bool AssemblyReferencesMvvmCross(Assembly assembly, string? mvvmCrossAssemblyName)
        {
            if (string.IsNullOrEmpty(mvvmCrossAssemblyName))
                return false;

            try
            {
                return Array.Exists(assembly.GetReferencedAssemblies(), a => a.Name == mvvmCrossAssemblyName);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            if (pluginManager == null)
                throw new ArgumentNullException(nameof(pluginManager));

            var pluginAttribute = typeof(MvxPluginAttribute);
            var pluginAssemblies = GetPluginAssemblies();

            // Search Assemblies for Plugins
            foreach (var pluginAssembly in pluginAssemblies)
            {
                var assemblyTypes = pluginAssembly.ExceptionSafeGetTypes();

                // Search Types for Valid Plugin
                foreach (var type in assemblyTypes.Where(TypeContainsPluginAttribute))
                {
                    // Ensure Plugin has been loaded
                    pluginManager.EnsurePluginLoaded(type);
                }
            }

            bool TypeContainsPluginAttribute(Type type) =>
                type.GetCustomAttributes(pluginAttribute, false).Length > 0;
        }

        protected virtual ICrossApplication? CreateMvxApplication(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossApplication>();
        }

        protected virtual ICrossApplication? InitializeMvxApplication(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var app = CreateMvxApplication(iocProvider);
            if (app != null)
                iocProvider.RegisterSingleton<ICrossViewModelLocatorCollection>(app);
            return app;
        }

        protected virtual void InitializeApp(ICrossApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            SetupLog?.Log(LogLevel.Trace, "Setup: Application Initialize - On background thread");
            app.Initialize();
        }

        protected virtual ICrossViewsContainer InitializeViewsContainer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var container = CreateViewsContainer(iocProvider);
            iocProvider.RegisterSingleton(container);
            return container;
        }

        protected virtual void InitializeViewDispatcher(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var dispatcher = CreateViewDispatcher();
            iocProvider.RegisterSingleton(dispatcher);
            iocProvider.RegisterSingleton<ICrossMainThreadAsyncDispatcher>(dispatcher);
            iocProvider.RegisterSingleton<ICrossMainThreadDispatcher>(dispatcher);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual ICrossNavigationService? InitializeNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            CreateViewModelLoader(iocProvider);
            var navigationService = CreateNavigationService(iocProvider);
            if (navigationService != null)
            {
                SetupLog?.Log(LogLevel.Trace, "Setup: Load navigation routes");
                LoadNavigationServiceRoutes(navigationService, iocProvider);
            }
            return navigationService;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual void LoadNavigationServiceRoutes(ICrossNavigationService navigationService, IMvxIoCProvider iocProvider)
        {
            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));

            ValidateArguments(iocProvider);

            navigationService.LoadRoutes(GetViewModelAssemblies());
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            if (ViewAssemblies.Count == 0)
                ViewAssemblies.Add(GetType().GetTypeInfo().Assembly);

            return ViewAssemblies;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = _iocProvider?.Resolve<ICrossApplication>();
            if (app == null) return [];
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return [assembly];
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            return GetViewAssemblies().Distinct();
        }

        protected virtual IMvxResultViewModelManager? InitializeResultViewModelManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateResultViewModelManager(iocProvider);
        }

        protected virtual IMvxResultViewModelManager? CreateResultViewModelManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<IMvxResultViewModelManager>();
        }

        protected abstract ICrossNameMapping CreateViewToViewModelNaming();

        protected virtual ICrossViewModelByNameLookup? CreateViewModelByNameLookup(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossViewModelByNameLookup>();
        }

        protected virtual ICrossViewModelByNameRegistry? CreateViewModelByNameRegistry(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossViewModelByNameRegistry>();
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual ICrossNameMapping InitializeViewModelTypeFinder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            CreateViewModelByNameLookup(iocProvider);
            var viewModelByNameRegistry = CreateViewModelByNameRegistry(iocProvider);
            if (viewModelByNameRegistry != null)
            {
                var viewModelAssemblies = GetViewModelAssemblies();
                foreach (var assembly in viewModelAssemblies)
                {
                    viewModelByNameRegistry.AddAll(assembly);
                }
            }

            var nameMappingStrategy = CreateViewToViewModelNaming();
            iocProvider.RegisterSingleton(nameMappingStrategy);
            return nameMappingStrategy;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual IDictionary<Type, Type>? InitializeLookupDictionary(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var viewAssemblies = GetViewAssemblies();
            var builder = iocProvider.Resolve<ICrossTypeToTypeLookupBuilder>();
            return builder?.Build(viewAssemblies);
        }

        protected virtual ICrossViewsContainer? InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup,
            IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var container = iocProvider.Resolve<ICrossViewsContainer>();
            container?.AddAll(viewModelViewLookup);
            return container;
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void InitializeLastChance(IMvxIoCProvider iocProvider)
        {
            // always the very last thing to get initialized
            // base class implementation is empty by default
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }

        private void FireStateChange(CrossSetupState state)
        {
            StateChanged?.Invoke(this, new CrossSetupStateEventArgs(state));
        }

        protected static void ValidateArguments(IMvxIoCProvider iocProvider)
        {
            ArgumentNullException.ThrowIfNull(iocProvider);
        }
    }
}