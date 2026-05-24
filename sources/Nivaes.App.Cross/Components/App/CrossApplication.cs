using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

public abstract class CrossApplication 
    : ICrossApplication
{
    private ICrossViewModelLocator? _defaultLocator;

    private ICrossViewModelLocator DefaultLocator
    {
        get
        {
            _defaultLocator ??= CreateDefaultViewModelLocator();
            return _defaultLocator;
        }
    }

    protected virtual ICrossViewModelLocator CreateDefaultViewModelLocator()
    {
        return new CrossDefaultViewModelLocator();
    }

    public virtual void LoadPlugins(IMvxPluginManager pluginManager)
    {
        // do nothing
    }

    /// <summary>
    /// Any initialization steps that can be done in the background
    /// </summary>
    public virtual void Initialize()
    {
        // do nothing
    }

    /// <summary>
    /// Any initialization steps that need to be done on the UI thread
    /// </summary>
    public virtual Task Startup()
    {
        CrossLogHost.Default?.Log(LogLevel.Trace, "AppStart: Application Startup - On UI thread");
        return Task.CompletedTask;
    }

    /// <summary>
    /// If the application is restarted (eg primary activity on Android 
    /// can be restarted) this method will be called before Startup
    /// is called again
    /// </summary>
    public virtual void Reset()
    {
        // do nothing
    }

    public ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request)
    {
        return DefaultLocator;
    }

    //protected void RegisterCustomAppStart<
    //    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TMvxAppStart>()
    //        where TMvxAppStart : class, ICrossAppStart
    //{
    //    //Mvx.IoCProvider?.ConstructAndRegisterSingleton<ICrossAppStart, TMvxAppStart>();
    //}

    protected void RegisterAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>()
        where TViewModel : ICrossViewModel
    {
        var container = Singleton<CrossIoCServiceContainer>.Instance;
        container.AddDelegate<ICrossAppStart>(container =>
        {
            var application = container.Resolve<ICrossApplication>();
            var navigationService = container.Resolve<ICrossNavigationService>();
            return new CrossAppStart<TViewModel>(application, navigationService);
        });
    }

    //protected void RegisterAppStart(ICrossAppStart appStart)
    //{
    //    throw new NotImplementedException();
    //    //Mvx.IoCProvider?.RegisterSingleton(appStart);
    //}

    //protected virtual void RegisterAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel, TParameter>()
    //  where TViewModel : ICrossViewModel<TParameter> where TParameter : class
    //{
    //    throw new NotImplementedException();
    //    //Mvx.IoCProvider?.ConstructAndRegisterSingleton<ICrossAppStart, MvxAppStart<TViewModel, TParameter>>();
    //}

    //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    //[Obsolete("No usar reflection", true)]
    //protected IEnumerable<Type> CreatableTypes()
    //{
    //    return CreatableTypes(GetType().GetTypeInfo().Assembly);
    //}

    //[RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    //[Obsolete("No usar reflection")]
    //protected IEnumerable<Type> CreatableTypes(Assembly assembly)
    //{
    //    return assembly.CreatableTypes();
    //}
}

public class CrossApplication<TParameter> 
    : CrossApplication, ICrossApplication<TParameter>
{
    public virtual Task<TParameter> Startup(TParameter hint)
    {
        return Task.FromResult(hint);
    }
}
