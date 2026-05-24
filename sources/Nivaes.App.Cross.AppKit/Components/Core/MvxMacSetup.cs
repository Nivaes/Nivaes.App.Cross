namespace Nivaes.App.Cross.AppKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Mac.Presenters;
    using Nivaes.App.Cross.Components.Hosting;
    using Nivaes.IoC;

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

        protected sealed override ICrossViewsContainer CreateViewsContainer()
        {
            var container = CreateMacViewsContainer();
            throw new NotImplementedException("Carga de views");
            //RegisterMacViewCreator(iocProvider, container);
            return container;
        }

        protected virtual IMvxMacViewsContainer CreateMacViewsContainer()
        {
            return new MvxMacViewsContainer();
        }

        protected virtual void RegisterMacViewCreator(IMvxIoCProvider iocProvider, IMvxMacViewsContainer container)
        {
            iocProvider.RegisterSingleton<IMvxMacViewCreator>(container);
            iocProvider.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            return new MvxMacViewDispatcher(_presenter);
        }

        protected override void InitializeFirstChance()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            RegisterPresenter(container);
            RegisterLifetime(container);
            base.InitializeFirstChance();
        }

        protected virtual void RegisterLifetime(CrossIoCServiceContainer container)
        {
            container.AddInstance<ICrossLifetime>(_applicationDelegate);
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

        protected virtual void RegisterPresenter(CrossIoCServiceContainer container)
        {
            var presenter = Presenter;
            container.AddInstance(presenter);
            container.AddInstance<ICrossViewPresenter>(presenter);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual CrossBindingBuilder CreateBindingBuilder()
        {
            return new MvxMacBindingBuilder(FillTargetFactories, FillValueConverters, FillBindingNames,
                FillValueCombiners);
        }

        protected virtual void FillBindingNames(ICrossBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        [Obsolete("No usar reflection", true)]
        protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        [Obsolete("No usar reflection", true)]
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

        [Obsolete("No usar reflection", true)]
        protected virtual IEnumerable<Type> ValueConverterHolders => Array.Empty<Type>();

        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }

    public abstract class MvxMacSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApplication> : MvxMacSetup
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
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
#nullable restore
}
