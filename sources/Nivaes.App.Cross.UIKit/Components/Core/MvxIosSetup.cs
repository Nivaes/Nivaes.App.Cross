////namespace Nivaes.App.Cross.UIKitOS
////{
////    using System.Diagnostics.CodeAnalysis;
////    using System.Reflection;
////    using Microsoft.Extensions.Logging;
////    using MvvmCross.IoC;
////    using Nivaes.App.Cross.Components.Hosting;
////    using Nivaes.IoC;

////    public abstract class MvxIosSetup
////    : CrossSetup, IMvxIosSetup
////    {
////        protected ICrossLifetime? LifetimeInstance { get; private set; }
////        protected UIWindow? Window { get; private set; }

////        private IMvxIosViewPresenter? _presenter;

////        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window)
////        {
////            Window = window;
////            LifetimeInstance = lifetimeInstance;
////        }

////        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, IMvxIosViewPresenter presenter)
////        {
////            LifetimeInstance = lifetimeInstance;
////            _presenter = presenter;
////        }

////        protected sealed override ICrossViewsContainer CreateViewsContainer()
////        {
////            var container = CreateIosViewsContainer();
////            throw new NotImplementedException("Carga de vies");
////            //RegisterIosViewCreator(iocProvider, container);
////            return container;
////        }

////        protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
////        {
////            return new MvxIosViewsContainer();
////        }

////        protected virtual void RegisterIosViewCreator(IMvxIoCProvider iocProvider, IMvxIosViewsContainer container)
////        {
////            iocProvider.RegisterSingleton<IMvxIosViewCreator>(container);
////            iocProvider.RegisterSingleton<ICrossCurrentRequest>(container);
////        }

////        protected override ICrossViewDispatcher CreateViewDispatcher()
////        {
////            return new MvxIosViewDispatcher(Presenter);
////        }

////        protected override void InitializeFirstChance()
////        {
////            var container = Singleton<CrossIoCServiceContainer>.Instance;
////            RegisterPlatformProperties(container);
////            RegisterPresenter(container);
////#if IOS || MACCATALYST
////            RegisterPopoverPresentationSourceProvider(container);
////#endif
////            RegisterLifetime(container);
////            base.InitializeFirstChance();
////        }

////        protected virtual void RegisterPlatformProperties(CrossIoCServiceContainer container)
////        {
////            container.AddInstance<IMvxIosSystem>(CreateIosSystemProperties());
////        }

////        protected virtual MvxIosSystem CreateIosSystemProperties()
////        {
////            return new MvxIosSystem();
////        }

////        protected virtual void RegisterLifetime(CrossIoCServiceContainer container)
////        {

////            if (LifetimeInstance == null)
////            {
////                SetupLog?.LogError(
////                    "ApplicationDelegate is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
////                    nameof(RegisterLifetime), nameof(PlatformInitialize));
////                return;
////            }

////            container.AddInstance<ICrossLifetime>(LifetimeInstance);
////        }

////        protected IMvxIosViewPresenter? Presenter
////        {
////            get
////            {
////                _presenter ??= CreateViewPresenter();
////                return _presenter;
////            }
////        }

////        protected virtual IMvxIosViewPresenter? CreateViewPresenter()
////        {
////            if (Window == null)
////            {
////                SetupLog?.LogError(
////                    "Window is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
////                    nameof(CreateViewPresenter), nameof(PlatformInitialize));
////                return null;
////            }

////            return new MvxIosViewPresenter(Window);
////        }

////        protected virtual void RegisterPresenter(CrossIoCServiceContainer container)
////        {
////            if (Presenter == null)
////            {
////                SetupLog?.LogError("Presenter is null in {MethodName}. Make sure to call {CreatePresenterMethodName}",
////                    nameof(RegisterPresenter), nameof(CreateViewPresenter));

////                return;
////            }

////            var presenter = Presenter;
////            container.AddInstance(presenter);
////            container.AddInstance<ICrossViewPresenter>(presenter);
////        }

////#if IOS || MACCATALYST
////        protected virtual void RegisterPopoverPresentationSourceProvider(CrossIoCServiceContainer container)
////        {
////            container.AddInstance(CreatePopoverPresentationSourceProvider());
////        }

////        protected virtual IMvxPopoverPresentationSourceProvider CreatePopoverPresentationSourceProvider()
////        {
////            return new MvxPopoverPresentationSourceProvider();
////        }
////#endif

////        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
////        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
////        {
////            var bindingBuilder = CreateBindingBuilder();
////            bindingBuilder.DoRegistration(iocProvider);
////        }

////        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
////        protected virtual CrossBindingBuilder CreateBindingBuilder()
////        {
////            return new MvxIosBindingBuilder(FillTargetFactories, FillValueConverters, FillValueCombiners,
////                FillBindingNames);
////        }

////        protected virtual void FillBindingNames(ICrossBindingNameRegistry obj)
////        {
////            // this base class does nothing
////        }

////        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
////        [Obsolete("No usar reflection")]
////        protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
////        {
////            registry.Fill(ValueConverterAssemblies);
////            registry.Fill(ValueConverterHolders);
////        }

////        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
////        {
////            // this base class does nothing
////        }

////        protected virtual List<Type> ValueConverterHolders => new List<Type>();

////        [Obsolete("No usar reflection")]
////        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
////        {
////            [Obsolete("No usar reflection")]
////            [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
////            get
////            {
////                var toReturn = new List<Assembly>();
////                toReturn.AddRange(GetViewModelAssemblies());
////                toReturn.AddRange(GetViewAssemblies());
////                return toReturn;
////            }
////        }

////        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
////        {
////            // this base class does nothing
////        }

////        protected override ICrossNameMapping CreateViewToViewModelNaming()
////        {
////            return new CrossPostfixAwareViewToViewModelNameMapping("View", "ViewController");
////        }
////    }

////    public abstract class MvxIosSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxIosSetup
////        where TApplication : class, ICrossApplication, new()
////    {
////        protected override void CreateApp()
////        {
////            var container = Singleton<CrossIoCServiceContainer>.Instance;
////            container.AddDelegate<ICrossApplication>(container =>
////            {
////                return new TApplication();
////            });
////        }

////        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
////        public override IEnumerable<Assembly> GetViewModelAssemblies()
////        {
////            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
////        }
////    }
////}