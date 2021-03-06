﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Autofac;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using MvvmCross.Plugin;
    using MvvmCross.Views;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.IoC;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxSetup
        : IMvxSetup
    {
        protected static Action<IIoCProvider>? RegisterSetupDependencies { get; set; }

        [Obsolete("Generar por Roslyn.")]
        protected static Func<IMvxSetup>? SetupCreator { get; set; }

        [Obsolete("Generar por Roslyn.", true)]
        protected static IEnumerable<Assembly>? ViewAssemblies { get; set; }

        //protected IMvxLog? SetupLog { get; private set; }

        //[Obsolete("Generar por Roslyn.", true)]
        //public static void RegisterSetupType<TMvxSetup>(params Assembly[] assemblies)
        //    where TMvxSetup : MvxSetup, new()
        //{
        //    ViewAssemblies = assemblies;
        //    if (!(ViewAssemblies?.Any() ?? false))
        //    {
        //        // fall back to all assemblies. Assembly.GetEntryAssembly() always returns
        //        // null on Xamarin platforms do not use it!
        //        ViewAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        //    }

        //    // Avoid creating the instance of Setup right now, instead
        //    // take a reference to the type in a way that we can avoid
        //    // using reflection to create the instance.
        //    SetupCreator = () => new TMvxSetup();
        //}

        //[Obsolete("Generar por Roslyn.", true)]
        //public static IMvxSetup Instance()
        //{
        //    var instance = SetupCreator?.Invoke(); // ?? (TSetup)Activator.CreateInstance(typeof(TMvxSetup));  /*MvxSetupExtensions.CreateSetup<MvxSetup>()*/;
        //    return instance;
        //}

        public static IMvxSetup Instance<TMvxSetup>()
             where TMvxSetup : MvxSetup, new()
        {
            return new TMvxSetup();
        }

        protected abstract ICrossApplication CreateApp();

        protected abstract IMvxViewsContainer CreateViewsContainer();

        protected abstract IMvxViewDispatcher CreateViewDispatcher();

        public Task StartSetupInitialization()
        {
            return InitializePrimary()
                .ContinueWith(async t =>
                {
                    if (t.IsCompleted)
                    {
                        await InitializeSecondary().ConfigureAwait(true);
                    }
                    else
                    {
                        throw new MvxException($"'{nameof(InitializePrimary)}' is not completed successfully.");
                    }
                }, TaskScheduler.Default);
        }

        public virtual async Task InitializePrimary()
        {
            if (State != MvxSetupState.Uninitialized)
            {
                throw new MvxException("The primary is initialized.");
            }

            State = MvxSetupState.InitializingPrimary;
            var iocProvider = InitializeIoC();

            IoCContainer.RegisterTypes(containerBuilder =>
            {
                RegisterInternalDependencies(containerBuilder);
                RegisterDependencies(containerBuilder);
            });

            RegisterDefaultSetupDependencies(iocProvider);
            RegisterSetupDependencies?.Invoke(iocProvider);

            var taskResult = Task.WhenAll(new[]
            {
                InitializeFirstChance(),
                InitializeViewDispatcher(),
                InitializeApp(),
                InitializeViewModelTypeFinder(),
            });

            try
            {
                await taskResult.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (taskResult.IsCanceled)
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializePrimary),
                        ["Cause"] = "IsCanceled"
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
                else if (taskResult.IsFaulted)
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializePrimary),
                        ["Cause"] = "IsFaulted"
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
                else
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializePrimary),
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
            }

            State = MvxSetupState.InitializedPrimary;
        }

        public virtual async Task InitializeSecondary()
        {
            if (State != MvxSetupState.InitializedPrimary)
            {
                throw new MvxException("The primary is not completed.");
            }

            State = MvxSetupState.InitializingSecondary;

            var taskResult = Task.WhenAll(new[]
            {
                PerformBootstrapActions(),
                InitializeNavigationService(),

                InitializeViewsContainer(),
                InitializeViewLookup(),

                InitializeCommandCollectionBuilder(),
                InitializeNavigationSerializer(),
                InitializeLastChance()
            });

            try
            {
                await taskResult.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (taskResult.IsCanceled)
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializeSecondary),
                        ["Cause"] = "IsCanceled"
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
                else if (taskResult.IsFaulted)
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializeSecondary),
                        ["Cause"] = "IsFaulted"
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
                else
                {
                    var propeties = new Dictionary<string, string>()
                    {
                        ["Origin"] = nameof(MvxSetup),
                        ["Method"] = nameof(InitializeSecondary),
                    };

                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, propeties);
                }
            }

            //SetupLog?.Trace("Setup: Secondary end");
            State = MvxSetupState.Initialized;
        }

        private void RegisterInternalDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MvxSettings>().As<IMvxSettings>();
            containerBuilder.RegisterType<MvxStringToTypeParser>().As<IMvxStringToTypeParser>();
        }

        protected abstract void RegisterDependencies(ContainerBuilder containerBuilder);

        protected virtual IMvxChildViewModelCache CreateViewModelCache()
        {
            return Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
        }

        protected virtual Task PerformBootstrapActions()
        {
            return Task.Run(() =>
            {
                var bootstrapRunner = new MvxBootstrapRunner();
                foreach (var assembly in GetBootstrapOwningAssemblies())
                {
                    bootstrapRunner.Run(assembly);
                }
            });
        }

        protected virtual Task InitializeNavigationSerializer()
        {
            return Task.Run(() =>
            {
                _ = CreateNavigationSerializer();
            });
        }

        protected virtual IMvxNavigationSerializer CreateNavigationSerializer()
        {
            return Mvx.IoCProvider.Resolve<IMvxNavigationSerializer>();
        }

        protected virtual Task InitializeCommandCollectionBuilder()
        {
            return Task.Run(() =>
            {
                //SetupLog?.Trace("Setup: CommandCollectionBuilder start");
                _ = CreateCommandCollectionBuilder();
            });
        }

        protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder()
        {
            return Mvx.IoCProvider.Resolve<IMvxCommandCollectionBuilder>();
        }

        protected virtual IIoCProvider InitializeIoC()
        {
            // initialize the IoC registry, then add it to itself
            var iocProvider = CreateIocProvider();
            Mvx.IoCProvider.RegisterSingleton(iocProvider);
            Mvx.IoCProvider.RegisterSingleton<IMvxSetup>(this);
            return iocProvider;
        }

        protected virtual void RegisterDefaultSetupDependencies(IIoCProvider iocProvider)
        {
            if (iocProvider == null) throw new NullReferenceException(nameof(iocProvider));


            //RegisterLogProvider(iocProvider);
            //iocProvider.LazyConstructAndRegisterSingleton<IMvxSettings, MvxSettings>();
            //iocProvider.LazyConstructAndRegisterSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();

            iocProvider.RegisterSingleton<IMvxPluginManager>(() => new MvxPluginManager(GetPluginConfiguration));
            iocProvider.RegisterSingleton(CreateApp);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationService, IMvxViewModelLoader>(loader => new MvxNavigationService(null, loader));
            iocProvider.RegisterSingleton(() => new MvxViewModelByNameLookup());
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameLookup, MvxViewModelByNameLookup>(nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelByNameRegistry, MvxViewModelByNameLookup>(nameLookup => nameLookup);
            iocProvider.LazyConstructAndRegisterSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxTypeToTypeLookupBuilder, MvxViewModelViewLookupBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxCommandCollectionBuilder, MvxCommandCollectionBuilder>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();
            iocProvider.LazyConstructAndRegisterSingleton<IMvxChildViewModelCache, MvxChildViewModelCache>();

            iocProvider.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions();
        }

        protected virtual IIoCProvider CreateIocProvider()
        {
            return IoCProvider.Initialize(CreateIocOptions());
        }

        [Obsolete("Sustituir por register.")]
        protected virtual Task InitializeFirstChance()
        {
            return Task.CompletedTask;
        }

        //[Obsolete("Usar AppCenter", true)]
        //protected virtual Task InitializeLoggingServices()
        //{
        //    return Task.Run(() =>
        //    {
        //        var logProvider = CreateLogProvider();
        //        SetupLog = logProvider.GetLogFor<MvxSetup>();
        //        var globalLog = logProvider.GetLogFor<MvxLog>();
        //        MvxLog.Instance = globalLog;
        //        Mvx.IoCProvider.RegisterSingleton(globalLog);
        //    });
        //}

        //[Obsolete("Usar AppCenter", true)]
        //public virtual MvxLogProviderType GetDefaultLogProviderType()
        //    => MvxLogProviderType.AppCenter;

        //[Obsolete("Usar AppCenter", true)]
        //protected virtual void RegisterLogProvider(IIoCProvider iocProvider)
        //{
        //    Func<IMvxLogProvider>? logProviderCreator = (GetDefaultLogProviderType()) switch
        //    {
        //        MvxLogProviderType.Console => () => new ConsoleLogProvider(),
        //        //MvxLogProviderType.EntLib => () => new EntLibLogProvider(),
        //        //MvxLogProviderType.Log4Net => () => new Log4NetLogProvider(),
        //        //MvxLogProviderType.Loupe => () => new LoupeLogProvider(),
        //        //MvxLogProviderType.NLog => () => new NLogLogProvider(),
        //        //MvxLogProviderType.Serilog => () => new SerilogLogProvider(),
        //        MvxLogProviderType.AppCenter => () => new AppCenterLogProvider(),
        //        _ => null,
        //    };
        //    if (logProviderCreator != null)
        //    {
        //        iocProvider?.RegisterSingleton(logProviderCreator);
        //    }
        //}

        //[Obsolete("Usar AppCenter", true)]
        //protected virtual IMvxLogProvider CreateLogProvider()
        //{
        //    return Mvx.IoCProvider.Resolve<IMvxLogProvider>();
        //}

        protected virtual IMvxViewModelLoader CreateViewModelLoader()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
        }

        protected virtual IMvxNavigationService CreateNavigationService()
        {
            return Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        }

        protected virtual Task<IMvxPluginManager> InitializePluginFramework()
        {
            return Task.Run(() =>
            {
                var pluginManager = CreatePluginManager();
                LoadPlugins(pluginManager);
                return pluginManager;
            });
        }

        protected virtual IMvxPluginManager CreatePluginManager()
        {
            return Mvx.IoCProvider.Resolve<IMvxPluginManager>();
        }

        protected virtual IMvxPluginConfiguration? GetPluginConfiguration(Type plugin)
        {
            return null;
        }

        [Obsolete("Not user reflector")]
        public virtual IEnumerable<Assembly> GetPluginAssemblies()
        {
            var mvvmCrossAssemblyName = typeof(MvxPluginAttribute).Assembly.GetName().Name;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var pluginAssemblies =
                assemblies
                    .AsParallel()
                    .Where(asmb => AssemblyReferencesMvvmCross(asmb, mvvmCrossAssemblyName));

            return pluginAssemblies;
        }

        [Obsolete("Not user reflector")]
        private bool AssemblyReferencesMvvmCross(Assembly assembly, string mvvmCrossAssemblyName)
        {
            try
            {
                return assembly.GetReferencedAssemblies().Any(a => a.Name == mvvmCrossAssemblyName);
            }
            catch
            {
                // TODO: Should the response here be true or false? Surely if exception we should return false?
                return true;
            }
        }

        public virtual void LoadPlugins(IMvxPluginManager pluginManager)
        {
            if (pluginManager == null) throw new NullReferenceException(nameof(pluginManager));

            var pluginAttribute = typeof(MvxPluginAttribute);
            var pluginAssemblies = GetPluginAssemblies();

            //Search Assemblies for Plugins
            foreach (var pluginAssembly in pluginAssemblies)
            {
                var assemblyTypes = pluginAssembly.ExceptionSafeGetTypes();

                //Search Types for Valid Plugin
                foreach (var type in assemblyTypes)
                {
                    if (TypeContainsPluginAttribute(type))
                    {
                        //Ensure Plugin has been loaded
                        pluginManager.EnsurePluginLoaded(type);
                    }
                }
            }

            bool TypeContainsPluginAttribute(Type type) => (type.GetCustomAttributes(pluginAttribute, false)?.Length ?? 0) > 0;
        }

        protected virtual ICrossApplication CreateMvxApplication()
        {
            return Mvx.IoCProvider.Resolve<ICrossApplication>();
        }

        protected virtual Task<ICrossApplication> InitializeMvxApplication()
        {
            return Task.Run(() =>
            {
                var app = CreateMvxApplication();
                Mvx.IoCProvider.RegisterSingleton<IMvxViewModelLocatorCollection>(app);
                return app;
            });
        }

        private async Task InitializeApp()
        {
            //SetupLog?.Trace("Setup: PluginManagerFramework start");
            var pluginManager = await InitializePluginFramework().ConfigureAwait(false);

            //SetupLog?.Trace("Setup: Create App");
            var app = await InitializeMvxApplication().ConfigureAwait(false);

            //SetupLog?.Trace("Setup: App start");
            await InitializeApp(pluginManager, app).ConfigureAwait(false);
        }

        protected virtual ValueTask InitializeApp(IMvxPluginManager pluginManager, ICrossApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.LoadPlugins(pluginManager);
            //SetupLog?.Trace("Setup: Application Initialize - On background thread");
            return app.Initialize();
        }

        protected virtual Task InitializeViewsContainer()
        {
            return Task.Run(() =>
            {
                //SetupLog?.Trace("Setup: ViewsContainer start");

                var container = CreateViewsContainer();
                Mvx.IoCProvider.RegisterSingleton(container);
            });
        }

        protected virtual Task InitializeViewDispatcher()
        {
            var dispatcher = CreateViewDispatcher();

            return Task.Run(() =>
            {
                //SetupLog?.Trace("Setup: ViewDispatcher start");

                //ToDo: Investigar porque se registra 3 veces.

                Mvx.IoCProvider.RegisterSingleton(dispatcher);
                Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
                //Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadDispatcher>(dispatcher);
            });
        }

        protected virtual Task<IMvxNavigationService> InitializeNavigationService()
        {
            //SetupLog?.Trace("Setup: NavigationService");
            var loader = CreateViewModelLoader();

            return Task.Run(() =>
            {
                var navigationService = CreateNavigationService();
                //SetupLog?.Trace("Setup: Load navigation routes");
                LoadNavigationServiceRoutes(navigationService);
                return navigationService;
            });
        }

        protected virtual void LoadNavigationServiceRoutes(IMvxNavigationService navigationService)
        {
            if (navigationService == null) throw new NullReferenceException(nameof(navigationService));

            navigationService.LoadRoutes(GetViewModelAssemblies());
        }

        [Obsolete("Not user reflector")]
        public virtual IEnumerable<Assembly> GetViewAssemblies()
        {
            var assemblies = ViewAssemblies ?? new[] { GetType().GetTypeInfo().Assembly };
            return assemblies;
        }

        [Obsolete("Not user reflector")]
        public virtual IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var app = Mvx.IoCProvider.Resolve<ICrossApplication>();
            var assembly = app.GetType().GetTypeInfo().Assembly;
            return new[] { assembly };
        }

        [Obsolete("Not user reflector")]
        protected virtual IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            return GetViewAssemblies().Distinct();
        }

        protected abstract IMvxNameMapping CreateViewToViewModelNaming();

        protected virtual IMvxViewModelByNameLookup CreateViewModelByNameLookup()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelByNameLookup>();
        }

        protected virtual IMvxViewModelByNameRegistry CreateViewModelByNameRegistry()
        {
            return Mvx.IoCProvider.Resolve<IMvxViewModelByNameRegistry>();
        }

        protected virtual Task InitializeViewModelTypeFinder()
        {
            return Task.Run(() =>
            {
                var viewModelByNameLookup = CreateViewModelByNameLookup();
                var viewModelByNameRegistry = CreateViewModelByNameRegistry();

                var viewModelAssemblies = GetViewModelAssemblies();
                foreach (var assembly in viewModelAssemblies)
                {
                    viewModelByNameRegistry.AddAll(assembly);
                }

                var nameMappingStrategy = CreateViewToViewModelNaming();
                Mvx.IoCProvider.RegisterSingleton(nameMappingStrategy);
            });
        }

        private async Task InitializeViewLookup()
        {
            var lookup = await InitializeLookupDictionary().ConfigureAwait(false);

            _ = await InitializeViewLookup(lookup).ConfigureAwait(false);
        }

        protected virtual Task<IDictionary<Type, Type>> InitializeLookupDictionary()
        {
            return Task.Run(() =>
            {
                var viewAssemblies = GetViewAssemblies();
                var builder = Mvx.IoCProvider.Resolve<IMvxTypeToTypeLookupBuilder>();
                var viewModelViewLookup = builder.Build(viewAssemblies);
                return viewModelViewLookup;
            });
        }

        protected virtual Task<IMvxViewsContainer?> InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup)
        {
            return Task.Run(async () =>
            {
                if (viewModelViewLookup == null)
                    return null;

                await Task.Delay(100).ConfigureAwait(false);

                var container = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
                container.AddAll(viewModelViewLookup);
                return container;
            });
        }

        [Obsolete("Sustituir por register.")]
        protected virtual Task InitializeLastChance()
        {
            return Task.CompletedTask;
        }

        [Obsolete("Not user reflector")]
        public IEnumerable<Type> CreatableTypes()
        {
            return CreatableTypes(GetType().GetTypeInfo().Assembly);
        }

        [Obsolete("Not user reflector")]
        public IEnumerable<Type> CreatableTypes(Assembly assembly)
        {
            return assembly.CreatableTypes();
        }

        public enum MvxSetupState
        {
            Uninitialized,
            InitializingPrimary,
            InitializedPrimary,
            InitializingSecondary,
            Initialized
        }

        public class MvxSetupStateEventArgs
            : EventArgs
        {
            public MvxSetupStateEventArgs(MvxSetupState setupState)
            {
                SetupState = setupState;
            }

            public MvxSetupState SetupState { get; }
        }

        public event EventHandler<MvxSetupStateEventArgs>? StateChanged;

        private MvxSetupState mState;

        public MvxSetupState State
        {
            get => mState;
            private set
            {
                mState = value;
                FireStateChange(value);
            }
        }

        private void FireStateChange(MvxSetupState state)
        {
            StateChanged?.Invoke(this, new MvxSetupStateEventArgs(state));
        }
    }

    public abstract class MvxSetup<TApplication> : MvxSetup
        where TApplication : class, ICrossApplication, new()
    {
        protected override ICrossApplication CreateApp() => Mvx.IoCProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
