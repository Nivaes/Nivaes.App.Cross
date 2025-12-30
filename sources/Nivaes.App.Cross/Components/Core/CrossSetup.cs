namespace Nivaes.App.Cross
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;
    using MvvmCross.Plugin;
    using Nivaes.IoC;

    public abstract class CrossSetup
        : ICrossSetup
    {
        public event EventHandler<CrossSetupStateEventArgs>? StateChanged;

        private static readonly object Lock = new();
        private CrossSetupState _state;
        [Obsolete("Quitar MvxIoC")]
        private IMvxIoCProvider? _iocProvider;

        [Obsolete("Quitar MvxIoC")]
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

            CrossLogHost.Default?.LogInformation("Setup: RegisterSetupType already called");
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        public static ICrossSetup? Instance()
        {
            var instance = SetupCreator?.Invoke() ?? CrossSetupExtensions.CreateSetup<CrossSetup>();
            return instance;
        }

        protected abstract void CreateApp();

        protected abstract ICrossViewsContainer CreateViewsContainer();

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

                InitializeLoggingServices();

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
                //SetupLog?.Log(LogLevel.Trace, "Setup: MvvmCross settings start");
                //InitializeSettings();
                SetupLog?.Log(LogLevel.Trace, "Setup: Singleton Cache start");
                InitializeSingletonCache();
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewDispatcher start");
                InitializeViewDispatcher();
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
                //SetupLog?.Log(LogLevel.Trace, "Setup: Bootstrap actions");
                //PerformBootstrapActions();
                //SetupLog?.Log(LogLevel.Trace, "Setup: StringToTypeParser start");
                //InitializeStringToTypeParser(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: FillableStringToTypeParser start");
                //InitializeFillableStringToTypeParser(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: Create App");
                var app = InitializeMvxApplication();
                //SetupLog?.Log(LogLevel.Trace, "Setup: NavigationService");
                //InitializeNavigationService(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: ResultViewModelManager");
                //InitializeResultViewModelManager(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewModelTypeFinder start");
                InitializeViewModelTypeFinder();
                SetupLog?.Log(LogLevel.Trace, "Setup: ViewsContainer start");
                InitializeViewsContainer();
                SetupLog?.Log(LogLevel.Trace, "Setup: Lookup Dictionary start");
                InitializeViewLookup();
                //var lookup = InitializeLookupDictionary(_iocProvider);
                //if (lookup != null)
                //{
                //    SetupLog?.Log(LogLevel.Trace, "Setup: Views start");
                //    InitializeViewLookup(lookup, _iocProvider);
                //}
                //else
                //{
                //    SetupLog?.LogWarning("Lookup dictionary is null returning from {MethodName}",
                //        nameof(InitializeLookupDictionary));
                //}

                //SetupLog?.Log(LogLevel.Trace, "Setup: CommandCollectionBuilder start");
                //InitializeCommandCollectionBuilder(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: NavigationSerializer start");
                //InitializeNavigationSerializer(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                //InitializeInpcInterception(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: InpcInterception start");
                //InitializeViewModelCache(_iocProvider);
                SetupLog?.Log(LogLevel.Trace, "Setup: BindingBuilder start");
                InitializeBindingBuilder(_iocProvider);
                //SetupLog?.Log(LogLevel.Trace, "Setup: PluginManagerFramework start");
                //var pluginManager = InitializePluginFramework(_iocProvider);
                //if (pluginManager != null)
                //{
                //    app?.LoadPlugins(pluginManager);
                //    SetupLog?.Log(LogLevel.Trace, "Setup: App start");
                //}
                //else
                //{
                //    SetupLog?.LogWarning("PluginManager was null returning from {MethodName}",
                //        nameof(InitializePluginFramework));
                //}

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

        [Obsolete("No define nada", true)]
        protected virtual void InitializeInpcInterception(IMvxIoCProvider iocProvider)
        {
            // by default no Inpc calls are intercepted
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossChildViewModelCache? InitializeViewModelCache(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var cache = CreateViewModelCache(iocProvider);
            return cache;
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossChildViewModelCache? CreateViewModelCache(IMvxIoCProvider iocProvider)
        {
            return iocProvider.Resolve<ICrossChildViewModelCache>();
        }

        //protected virtual void InitializeSettings()
        //{
        //    var container = Singleton<CrossIoCServiceContainer>.Instance;
        //    container.AddDelegate<ICrossSettings>((container) =>
        //    {
        //        var settings = CreateSettings();
        //        return settings;
        //    });
        //}

        //protected virtual ICrossSettings? CreateSettings(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return iocProvider.Resolve<ICrossSettings>();
        //}

        //protected virtual ICrossStringToTypeParser? InitializeStringToTypeParser(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return CreateStringToTypeParser(iocProvider);
        //}

        //protected virtual ICrossStringToTypeParser? CreateStringToTypeParser(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return iocProvider.Resolve<ICrossStringToTypeParser>();
        //}

        //protected virtual ICrossFillableStringToTypeParser? InitializeFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    var parser = CreateFillableStringToTypeParser(iocProvider);
        //    if (parser != null)
        //        iocProvider.RegisterSingleton(parser);

        //    return parser;
        //}

        //protected virtual ICrossFillableStringToTypeParser? CreateFillableStringToTypeParser(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return iocProvider.Resolve<ICrossStringToTypeParser>() as ICrossFillableStringToTypeParser;
        //}

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar refelction", true)]
        protected virtual void PerformBootstrapActions()
        {
            var bootstrapRunner = new CrossBootstrapRunner();
            foreach (var assembly in GetBootstrapOwningAssemblies())
            {
                bootstrapRunner.Run(assembly);
            }
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossNavigationSerializer? InitializeNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateNavigationSerializer(iocProvider);
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossNavigationSerializer? CreateNavigationSerializer(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossNavigationSerializer>();
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossCommandCollectionBuilder? InitializeCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateCommandCollectionBuilder(iocProvider);
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossCommandCollectionBuilder? CreateCommandCollectionBuilder(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossCommandCollectionBuilder>();
        }

        [Obsolete("Quitar MvxIoC")]
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
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            //ValidateArguments(iocProvider);

            //container.AddDelegate<IMvxPluginManager>(container => new MvxPluginManager(iocProvider, GetPluginConfiguration))
            //iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(iocProvider, GetPluginConfiguration));

            CreateApp();
            //iocProvider.RegisterSingleton(CreateApp(iocProvider));
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelLoader, CrossViewModelLoader>();
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossNavigationService, ICrossViewModelLoader, ICrossViewDispatcher, IMvxIoCProvider>(
            //    (loader, dispatcher, p) => new CrossNavigationService(loader, dispatcher, p));
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossResultViewModelManager, CrossResultViewModelManager>();
            iocProvider.RegisterSingleton(() => new CrossViewModelByNameLookup());
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelByNameLookup, CrossViewModelByNameLookup>(
            //    nameLookup => nameLookup);
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelByNameRegistry, CrossViewModelByNameLookup>(
            //    nameLookup => nameLookup);
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossViewModelTypeFinder, CrossViewModelViewTypeFinder>();
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossTypeToTypeLookupBuilder, CrossViewModelViewLookupBuilder>();
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossCommandCollectionBuilder, CrossCommandCollectionBuilder>();
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossNavigationSerializer, CrossStringDictionaryNavigationSerializer>();
            //iocProvider.LazyConstructAndRegisterSingleton<ICrossChildViewModelCache, CrossChildViewModelCache>();

            iocProvider.RegisterType<ICrossCommandHelper, CrossWeakCommandHelper>();
        }

        [Obsolete("Quitar MvxIoC", true)]
        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        [Obsolete("Quitar MvxIoC", true)]
        protected virtual IMvxIoCProvider CreateIocProvider()
        {
            return MvxIoCProvider.Initialize(CreateIocOptions());
        }

        protected virtual void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            // always the very first thing to get initialized - after IoC and base platform
            // base class implementation is empty by default
        }

        protected virtual void InitializeLoggingServices()
        {
            // ToDo: Incluir esto en CrossIoCServiceContainer
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddDelegate<ILoggerProvider>((container) =>
            {
                var logProvider = CreateLogProvider();
                return logProvider;
            });


            container.AddDelegate<ILoggerFactory>((container) =>
            {
                var loggerFactory = CreateLogFactory();
                return loggerFactory;
            });
            SetupLog = container.Resolve<ILoggerFactory>()?.CreateLogger<CrossSetup>();
        }

        protected abstract ILoggerProvider? CreateLogProvider();
        protected abstract ILoggerFactory? CreateLogFactory();

        [Obsolete("Solo genera un recurso que no usa", true)]
        protected virtual ICrossViewModelLoader? CreateViewModelLoader(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossViewModelLoader>();
        }

        [Obsolete("Solo genera un recurso que no usa", true)]
        protected virtual ICrossNavigationService? CreateNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossNavigationService>();
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection para cargar plugins", true)]
        protected virtual IMvxPluginManager? InitializePluginFramework(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var pluginManager = CreatePluginManager(iocProvider);
            if (pluginManager != null)
                LoadPlugins(pluginManager);
            return pluginManager;
        }

        [Obsolete("Solo genera un recurso que no usa", true)]
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
        [Obsolete("No cargar plugins con reflection", true)]
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

        protected virtual ICrossApplication? CreateMvxApplication()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            return container.Resolve<ICrossApplication>();
        }

        protected virtual ICrossApplication? InitializeMvxApplication()
        {         
            var app = CreateMvxApplication();
            if (app != null)
            {
                var container = Singleton<CrossIoCServiceContainer>.Instance;
                container.AddInstance<ICrossViewModelLocatorCollection>(app);
            }
            return app;
        }

        protected virtual void InitializeApp(ICrossApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            SetupLog?.Log(LogLevel.Trace, "Setup: Application Initialize - On background thread");
            app.Initialize();
        }

        protected virtual ICrossViewsContainer InitializeViewsContainer()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            var viewContainer = CreateViewsContainer();
            container.AddInstance(viewContainer);
            return viewContainer;
        }

        protected virtual void InitializeViewDispatcher()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            var dispatcher = CreateViewDispatcher();

            container.AddInstance(dispatcher);
            container.AddInstance<ICrossMainThreadAsyncDispatcher>(dispatcher);
            container.AddInstance<ICrossMainThreadDispatcher>(dispatcher);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("Usa reflection para generar los mapas.", true)]
        protected virtual ICrossNavigationService? InitializeNavigationService(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            //CreateViewModelLoader(iocProvider);
            var navigationService = CreateNavigationService(iocProvider);
            if (navigationService != null)
            {
                SetupLog?.Log(LogLevel.Trace, "Setup: Load navigation routes");
                LoadNavigationServiceRoutes(navigationService, iocProvider);
            }
            return navigationService;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual void LoadNavigationServiceRoutes(ICrossNavigationService navigationService, IMvxIoCProvider iocProvider)
        {
            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));

            ValidateArguments(iocProvider);

            navigationService.LoadRoutes(GetViewModelAssemblies());
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            if (ViewAssemblies.Count == 0)
                ViewAssemblies.Add(GetType().GetTypeInfo().Assembly);

            return ViewAssemblies;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = _iocProvider?.Resolve<ICrossApplication>();
            if (app == null) return [];
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return [assembly];
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            return GetViewAssemblies().Distinct();
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossResultViewModelManager? InitializeResultViewModelManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return CreateResultViewModelManager(iocProvider);
        }

        [Obsolete("No define nada", true)]
        protected virtual ICrossResultViewModelManager? CreateResultViewModelManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            return iocProvider.Resolve<ICrossResultViewModelManager>();
        }

        [Obsolete("No asociar la vista y el modelo por el nombre de la clase")]
        protected abstract ICrossNameMapping CreateViewToViewModelNaming();

        //protected virtual ICrossViewModelByNameLookup? CreateViewModelByNameLookup(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return iocProvider.Resolve<ICrossViewModelByNameLookup>();
        //}

        //protected virtual ICrossViewModelByNameRegistry? CreateViewModelByNameRegistry(IMvxIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    return iocProvider.Resolve<ICrossViewModelByNameRegistry>();
        //}

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No asociar la vista y el modelo por el nombre de la clase")]
        protected virtual /*ICrossNameMapping*/ void InitializeViewModelTypeFinder()
        {
            //ValidateArguments(iocProvider);

            var container = Singleton<CrossIoCServiceContainer>.Instance;
            var viewModelByNameRegistry = (ICrossViewModelByNameRegistry?)container.Resolve<ICrossViewModelByNameLookup>();

            //CreateViewModelByNameLookup(iocProvider);
            //var viewModelByNameRegistry = CreateViewModelByNameRegistry(iocProvider);
            if (viewModelByNameRegistry != null)
            {
                var viewModelAssemblies = GetViewModelAssemblies();
                foreach (var assembly in viewModelAssemblies)
                {
                    viewModelByNameRegistry.AddAll(assembly);
                }
            }
            container.AddDelegate<ICrossNameMapping>((container) =>
                {
                    var nameMappingStrategy = CreateViewToViewModelNaming();
                    return nameMappingStrategy;
                });
            //iocProvider.RegisterSingleton(nameMappingStrategy);
            //return nameMappingStrategy;
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual IDictionary<Type, Type>? InitializeLookupDictionary(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var viewAssemblies = GetViewAssemblies();
            var builder = iocProvider.Resolve<ICrossTypeToTypeLookupBuilder>();
            return builder?.Build(viewAssemblies);
        }

        protected abstract void InitializeViewLookup();

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

        [Obsolete("No aporta nada")]
        protected static void ValidateArguments(IMvxIoCProvider iocProvider)
        {
            ArgumentNullException.ThrowIfNull(iocProvider);
        }
    }
}