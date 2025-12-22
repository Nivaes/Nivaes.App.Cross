namespace MvvmCross.Platforms.Mac.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Converters;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Mac.Binding;
    using MvvmCross.Platforms.Mac.Presenters;
    using MvvmCross.Platforms.Mac.Views;
    using Nivaes.App.Cross;

    public abstract class MvxMacSetup
        : CrossSetup, IMvxMacSetup
    {
        private IMvxApplicationDelegate? _applicationDelegate;
        private IMvxMacViewPresenter? _presenter;

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            PlatformInitialize(applicationDelegate);
            _presenter = presenter;
        }

        protected IMvxApplicationDelegate? ApplicationDelegate
        {
            get { return _applicationDelegate; }
        }

        protected override ICrossNameMapping CreateViewToViewModelNaming()
        {
            return new CrossPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        protected sealed override ICrossViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        {
            var container = CreateMacViewsContainer();
            RegisterMacViewCreator(iocProvider, container);
            return container;
        }

        protected virtual IMvxMacViewsContainer CreateMacViewsContainer()
        {
            return new MvxMacViewsContainer();
        }

        protected virtual void RegisterMacViewCreator(IMvxIoCProvider iocProvider, IMvxMacViewsContainer container)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<IMvxMacViewCreator>(container);
            iocProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            return new MvxMacViewDispatcher(_presenter);
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            RegisterPresenter(iocProvider);
            RegisterLifetime(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void RegisterLifetime(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            iocProvider.RegisterSingleton<ICrossLifetime>(_applicationDelegate);
        }

        protected IMvxMacViewPresenter Presenter
        {
            get
            {
                _presenter ??= CreateViewPresenter();
                return _presenter;
            }
        }

        protected virtual IMvxMacViewPresenter CreateViewPresenter()
        {
            return new MvxMacViewPresenter(_applicationDelegate);
        }

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var presenter = Presenter;
            iocProvider.RegisterSingleton(presenter);
            iocProvider.RegisterSingleton<ICrossViewPresenter>(presenter);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxMacBindingBuilder(FillTargetFactories, FillValueConverters, FillBindingNames,
                FillValueCombiners);
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual List<Assembly> ValueConverterAssemblies
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

        protected virtual IEnumerable<Type> ValueConverterHolders => Array.Empty<Type>();

        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }

    public abstract class MvxMacSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxMacSetup
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
#nullable restore
}
