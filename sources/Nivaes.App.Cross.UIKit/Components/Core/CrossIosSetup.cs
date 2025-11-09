namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;

    public abstract class CrossIosSetup
        : CrossSetup, ICrossIosSetup
    {
        protected ICrossLifetime? LifetimeInstance { get; private set; }
        protected UIWindow? Window { get; private set; }

        private ICrossIosViewPresenter? _presenter;

        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, UIWindow window)
        {
            Window = window;
            LifetimeInstance = lifetimeInstance;
        }

        public virtual void PlatformInitialize(ICrossLifetime lifetimeInstance, ICrossIosViewPresenter presenter)
        {
            LifetimeInstance = lifetimeInstance;
            _presenter = presenter;
        }

        //protected sealed override ICrossViewsContainer CreateViewsContainer(ICrossIoCProvider iocProvider)
        //{
        //    var container = CreateIosViewsContainer();
        //    RegisterIosViewCreator(iocProvider, container);
        //    return container;
        //}

        protected virtual ICrossIosViewsContainer CreateIosViewsContainer()
        {
            return new CrossIosViewsContainer();
        }

        protected virtual void RegisterIosViewCreator(ICrossIoCProvider iocProvider, ICrossIosViewsContainer container)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<ICrossIosViewCreator>(container);
            iocProvider.RegisterSingleton<ICrossCurrentRequest>(container);
        }

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            return new CrossIosViewDispatcher(Presenter);
        }

        protected override void InitializeFirstChance(ICrossIoCProvider iocProvider)
        {
            RegisterPlatformProperties(iocProvider);
            RegisterPresenter(iocProvider);
            //RegisterPopoverPresentationSourceProvider(iocProvider);
            RegisterLifetime(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void RegisterPlatformProperties(ICrossIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<ICrossIosSystem>(CreateIosSystemProperties());
        }

        protected virtual CrossIosSystem CreateIosSystemProperties()
        {
            return new CrossIosSystem();
        }

        protected virtual void RegisterLifetime(ICrossIoCProvider iocProvider)
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

        protected ICrossIosViewPresenter? Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual ICrossIosViewPresenter? CreateViewPresenter()
        {
            throw new NotImplementedException();
            //if (Window == null)
            //{
            //    SetupLog?.LogError(
            //        "Window is null in {MethodName}. Make sure to call {PlatformInitializeMethodName}",
            //        nameof(CreateViewPresenter), nameof(PlatformInitialize));
            //    return null;
            //}

            //return new MvxIosViewPresenter(Window);
        }

        protected virtual void RegisterPresenter(ICrossIoCProvider iocProvider)
        {
            throw new NotImplementedException();
            //ValidateArguments(iocProvider);

            //if (Presenter == null)
            //{
            //    SetupLog?.LogError("Presenter is null in {MethodName}. Make sure to call {CreatePresenterMethodName}",
            //        nameof(RegisterPresenter), nameof(CreateViewPresenter));

            //    return;
            //}

            //var presenter = Presenter;
            //iocProvider.RegisterSingleton(presenter);
            //iocProvider.RegisterSingleton<IMvxViewPresenter>(presenter);
        }

        //protected virtual void RegisterPopoverPresentationSourceProvider(ICrossIoCProvider iocProvider)
        //{
        //    ValidateArguments(iocProvider);

        //    iocProvider.RegisterSingleton(CreatePopoverPresentationSourceProvider());
        //}

        //protected virtual IMvxPopoverPresentationSourceProvider CreatePopoverPresentationSourceProvider()
        //{
        //    return new MvxPopoverPresentationSourceProvider();
        //}

        //[RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        //protected override void InitializeBindingBuilder(ICrossIoCProvider iocProvider)
        //{
        //    var bindingBuilder = CreateBindingBuilder();
        //    bindingBuilder.DoRegistration(iocProvider);
        //}

        //[RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        //protected virtual CrossBindingBuilder CreateBindingBuilder()
        //{
        //    return new CrossIosBindingBuilder(FillTargetFactories, FillValueConverters, FillValueCombiners,
        //        FillBindingNames);
        //}

        protected virtual void FillBindingNames(ICrossBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        //[RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        //protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
        //{
        //    registry.Fill(ValueConverterAssemblies);
        //    registry.Fill(ValueConverterHolders);
        //}

        //protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        //{
        //    // this base class does nothing
        //}

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

    public abstract class CrossIosSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : CrossIosSetup
        where TApplication : class, ICrossApplication, new()
    {
        //protected override ICrossApplication CreateApp(ICrossIoCProvider iocProvider) =>
        //    iocProvider.IoCConstruct<TApplication>();

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}