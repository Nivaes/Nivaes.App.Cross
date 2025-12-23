namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;

    public abstract class MvxIosSetup
    : CrossSetup, IMvxIosSetup
    {
        protected ICrossLifetime? LifetimeInstance { get; private set; }
        protected UIWindow? Window { get; private set; }

        private IMvxIosViewPresenter? _presenter;

        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window)
        {
            Window = window;
            LifetimeInstance = lifetimeInstance;
        }

        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, IMvxIosViewPresenter presenter)
        {
            LifetimeInstance = lifetimeInstance;
            _presenter = presenter;
        }

        protected sealed override ICrossViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            var container = CreateIosViewsContainer();
            RegisterIosViewCreator(iocProvider, container);
            return container;
        }

        protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new MvxIosViewsContainer();
        }

        protected virtual void RegisterIosViewCreator(IMvxIoCProvider iocProvider, IMvxIosViewsContainer container)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<IMvxIosViewCreator>(container);
            iocProvider.RegisterSingleton<ICrossCurrentRequest>(container);
        }

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            return new MvxIosViewDispatcher(Presenter);
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            RegisterPlatformProperties(iocProvider);
            RegisterPresenter(iocProvider);
#if IOS || MACCATALYST
            RegisterPopoverPresentationSourceProvider(iocProvider);
#endif
            RegisterLifetime(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void RegisterPlatformProperties(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        protected virtual void RegisterLifetime(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            if (LifetimeInstance == null)
            {
                SetupLog?.LogError(
                    "ApplicationDelegate is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
                    nameof(RegisterLifetime), nameof(PlatformInitialize));
                return;
            }

            iocProvider.RegisterSingleton<ICrossLifetime>(LifetimeInstance);
        }

        protected IMvxIosViewPresenter? Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxIosViewPresenter? CreateViewPresenter()
        {
            if (Window == null)
            {
                SetupLog?.LogError(
                    "Window is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
                    nameof(CreateViewPresenter), nameof(PlatformInitialize));
                return null;
            }

            return new MvxIosViewPresenter(Window);
        }

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            if (Presenter == null)
            {
                SetupLog?.LogError("Presenter is null in {MethodName}. Make sure to call {CreatePresenterMethodName}",
                    nameof(RegisterPresenter), nameof(CreateViewPresenter));

                return;
            }

            var presenter = Presenter;
            iocProvider.RegisterSingleton(presenter);
            iocProvider.RegisterSingleton<ICrossViewPresenter>(presenter);
        }

#if IOS || MACCATALYST
        protected virtual void RegisterPopoverPresentationSourceProvider(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton(CreatePopoverPresentationSourceProvider());
        }

        protected virtual IMvxPopoverPresentationSourceProvider CreatePopoverPresentationSourceProvider()
        {
            return new MvxPopoverPresentationSourceProvider();
        }
#endif

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual CrossBindingBuilder CreateBindingBuilder()
        {
            return new MvxIosBindingBuilder(FillTargetFactories, FillValueConverters, FillValueCombiners,
                FillBindingNames);
        }

        protected virtual void FillBindingNames(ICrossBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected override ICrossNameMapping CreateViewToViewModelNaming()
        {
            return new CrossPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }
    }

    public abstract class MvxIosSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxIosSetup
        where TApplication : class, ICrossApplication, new()
    {
        protected override ICrossApplication CreateApp(IMvxIoCProvider iocProvider) =>
            iocProvider.IoCConstruct<TApplication>();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}