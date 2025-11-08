namespace Nivaes.App.Cross.WinUI3
{
    using System.Reflection;
    using Microsoft.UI.Xaml.Controls;
    using MvvmCross.IoC;

    [Obsolete("use ViewPresenter")]
    public abstract class CrossWindowsSetup
        : CrossSetup, ICrossWindowsSetup
    {
        private ICrossWindowsFrame? _rootFrame;
        private string? _suspensionManagerSessionStateKey;
        //private ICrossWindowsViewPresenter? _presenter;

        public virtual void PlatformInitialize(Frame rootFrame, string activatedEventArgs,
            string? suspensionManagerSessionStateKey = null)
        {
            PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
            ActivationArguments = activatedEventArgs;
        }

        public virtual void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
        {
            throw new NotImplementedException();
            //PlatformInitialize(new CrossWrappedFrame(rootFrame));
            //_suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
        }

        public virtual void PlatformInitialize(ICrossWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;
        }

        public virtual void UpdateActivationArguments(string e)
        {
            ActivationArguments = e;
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            InitializeSuspensionManager(iocProvider);
            RegisterPresenter(iocProvider);
            base.InitializeFirstChance(iocProvider);
        }

        protected virtual void InitializeSuspensionManager(IMvxIoCProvider iocProvider)
        {
            ValidateArguments(iocProvider);

            var suspensionManager = CreateSuspensionManager();
            iocProvider.RegisterSingleton(suspensionManager);

            if (_suspensionManagerSessionStateKey != null)
                suspensionManager.RegisterFrame(_rootFrame, _suspensionManagerSessionStateKey);
        }

        protected virtual ICrossSuspensionManager CreateSuspensionManager()
        {
            return new CrossSuspensionManager();
        }

        //protected sealed override ICrossViewsContainer CreateViewsContainer(IMvxIoCProvider iocProvider)
        //{
        //    var container = CreateStoreViewsContainer();
        //    iocProvider.RegisterSingleton<ICrossWindowsViewModelRequestTranslator>(container);
        //    iocProvider.RegisterSingleton<ICrossWindowsViewModelLoader>(container);
        //    var viewsContainer = container as CrossViewsContainer;
        //    if (viewsContainer == null)
        //        throw new CrossException("CreateViewsContainer must return an CrossViewsContainer");
        //    return container;
        //}

        //protected virtual ICrossStoreViewsContainer CreateStoreViewsContainer()
        //{
        //    return new CrossWindowsViewsContainer();
        //}

        //protected ICrossWindowsViewPresenter Presenter
        //{
        //    get
        //    {
        //        if (_rootFrame == null)
        //            throw new InvalidOperationException("Cannot create View Presenter with null root frame");
        //        _presenter ??= CreateViewPresenter(_rootFrame);
        //        return _presenter;
        //    }
        //}

        //protected virtual ICrossWindowsViewPresenter CreateViewPresenter(ICrossWindowsFrame rootFrame)
        //{
        //    return new CrossMultiWindowViewPresenter(rootFrame);
        //}

        //protected virtual CrossWindowsViewDispatcher CreateViewDispatcher(ICrossWindowsFrame rootFrame)
        //{
        //    return new CrossWindowsViewDispatcher(Presenter, rootFrame);
        //}

        protected override ICrossViewDispatcher CreateViewDispatcher()
        {
            throw new NotImplementedException();
            //if (_rootFrame == null)
            //    throw new InvalidOperationException("Cannot create View Dispatcher with null root frame");
            //return CreateViewDispatcher(_rootFrame);
        }

        protected virtual void RegisterPresenter(IMvxIoCProvider iocProvider)
        {
            throw new NotImplementedException();

            //ValidateArguments(iocProvider);

            //var presenter = Presenter;
            //iocProvider.RegisterSingleton(presenter);
            //iocProvider.RegisterSingleton<ICrossViewPresenter>(presenter);
        }

        protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration(iocProvider);
        }

        protected virtual void FillBindingNames(ICrossBindingNameRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            // this base class does nothing
        }

        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected string? ActivationArguments { get; private set; }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual CrossBindingBuilder CreateBindingBuilder()
        {
            return new CrossWindowsBindingBuilder(FillTargetFactories, FillBindingNames, FillValueConverters, FillValueCombiners);
        }

        //protected override ICrossNameMapping CreateViewToViewModelNaming()
        //{
        //    return new CrossPostfixAwareViewToViewModelNameMapping("View", "Page");
        //}
    }

    [Obsolete("No es AoT compatible")]
    public abstract class CrossWindowsSetup<TApplication> : CrossWindowsSetup
         where TApplication : class, ICrossApplication, new()
    {
        //protected override ICrossApplication CreateApp(IMvxIoCProvider iocProvider) =>
        //    iocProvider.IoCConstruct<TApplication>();

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            return new[] { typeof(TApplication).GetTypeInfo().Assembly };
        }
    }
}
