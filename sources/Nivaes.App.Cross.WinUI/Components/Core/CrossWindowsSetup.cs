using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using MvvmCross.IoC;
using Nivaes.App.Cross.Hosting;
using Nivaes.IoC;

namespace Nivaes.App.Cross.WinUI;

[Obsolete]
public abstract class CrossWindowsSetup
    : CrossSetup, ICrossWindowsSetup
{
    private ICrossWindowsFrame? _rootFrame;
    private string? _suspensionManagerSessionStateKey;
    private IMvxWindowsViewPresenter? _presenter;

    public virtual void PlatformInitialize(Frame rootFrame, string activatedEventArgs,
        string? suspensionManagerSessionStateKey = null)
    {
        PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
        ActivationArguments = activatedEventArgs;
    }

    public virtual void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
    {
        PlatformInitialize(new CrossWrappedFrame(rootFrame));
        _suspensionManagerSessionStateKey = suspensionManagerSessionStateKey;
    }

    public virtual void PlatformInitialize(ICrossWindowsFrame rootFrame)
    {
        _rootFrame = rootFrame;
    }

    public virtual void UpdateActivationArguments(string e)
    {
        ActivationArguments = e;
    }

    protected override void InitializeFirstChance()
    {
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        InitializeSuspensionManager();
        RegisterPresenter();
        base.InitializeFirstChance();
    }

    protected virtual void InitializeSuspensionManager()
    {

        var suspensionManager = CreateSuspensionManager();
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.AddInstance<ICrossSuspensionManager>(suspensionManager);

        if (_suspensionManagerSessionStateKey != null)
            suspensionManager.RegisterFrame(_rootFrame, _suspensionManagerSessionStateKey);
    }

    protected virtual ICrossSuspensionManager CreateSuspensionManager()
    {
        return new CrossSuspensionManager();
    }

    [Obsolete("", true)]
    protected sealed override ICrossViewsContainer CreateViewsContainer()
    {
        var viewContainer = CreateStoreViewsContainer();
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.AddInstance<ICrossWindowsViewModelRequestTranslator>(viewContainer);
        container.AddInstance<ICrossWindowsViewModelLoader>(viewContainer);
        var viewsContainer = viewContainer as CrossViewsContainer;
        if (viewsContainer == null)
            throw new CrossException("CreateViewsContainer must return an MvxViewsContainer");
        return viewContainer;
    }

    [Obsolete("", true)]
    protected virtual ICrossStoreViewsContainer CreateStoreViewsContainer()
    {
        return null;
        //return new CrossWindowsViewsContainer();
    }

    [Obsolete("", true)]
    protected IMvxWindowsViewPresenter Presenter
    {
        get
        {
            if (_rootFrame == null)
                throw new InvalidOperationException("Cannot create View Presenter with null root frame");
            _presenter ??= CreateViewPresenter(_rootFrame);
            return _presenter;
        }
    }

    [Obsolete("", true)]
    protected virtual IMvxWindowsViewPresenter CreateViewPresenter(ICrossWindowsFrame rootFrame)
    {
        return null;
        //return new MvxMultiWindowViewPresenter(rootFrame);
    }

    protected virtual CrossWindowsViewDispatcher CreateViewDispatcher(ICrossWindowsFrame rootFrame)
    {
        return new CrossWindowsViewDispatcher(Presenter, rootFrame);
    }

    protected override ICrossViewDispatcher CreateViewDispatcher()
    {
        if (_rootFrame == null)
            throw new InvalidOperationException("Cannot create View Dispatcher with null root frame");
        return CreateViewDispatcher(_rootFrame);
    }

    [Obsolete("", true)]
    protected virtual void RegisterPresenter()
    {
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        var presenter = Presenter;
        container.AddInstance(presenter);
        container.AddInstance<ICrossViewPresenter>(presenter);
    }

    [Obsolete("No usar reflection", true)]
    protected override void InitializeBindingBuilder(IMvxIoCProvider iocProvider)
    {
        var bindingBuilder = CreateBindingBuilder();
        bindingBuilder.DoRegistration(iocProvider);
    }

    protected virtual void FillBindingNames(ICrossBindingNameRegistry registry)
    {
        // this base class does nothing
    }

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

    protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
    {
        // this base class does nothing
    }

    protected string? ActivationArguments { get; private set; }

    [Obsolete("No usar reflection", true)]
    protected virtual List<Type> ValueConverterHolders => new List<Type>();

    [Obsolete("No usar reflection", true)]
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

    [Obsolete("No usar reflection", true)]
    protected virtual CrossBindingBuilder CreateBindingBuilder()
    {
        return new MvxWindowsBindingBuilder(FillTargetFactories, FillBindingNames, FillValueConverters, FillValueCombiners);
    }

    protected override ICrossNameMapping CreateViewToViewModelNaming()
    {
        return new CrossPostfixAwareViewToViewModelNameMapping("View", "Page");
    }
}

[Obsolete]
public abstract class MvxWindowsSetup<TApplication> : CrossWindowsSetup
     where TApplication : class, ICrossApp, new()
{
    [Obsolete]
    protected override void CreateApp()
    {
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.AddDelegate<ICrossApp>(container =>
        {
            return new TApplication();
        });
    }

    public override IEnumerable<Assembly> GetViewModelAssemblies()
    {
        return new[] { typeof(TApplication).GetTypeInfo().Assembly };
    }
}
#nullable restore

