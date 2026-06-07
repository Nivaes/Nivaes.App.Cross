using Android.Content;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

public class MvxAndroidViewFactory
    : IMvxAndroidViewFactory
{
    private IMvxViewTypeResolver? _viewTypeResolver;
    private readonly IServiceProvider _serviceProvider;

    protected IMvxViewTypeResolver? ViewTypeResolver => _viewTypeResolver ??= Mvx.IoCProvider?.Resolve<IMvxViewTypeResolver>();

    public MvxAndroidViewFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public virtual View? CreateView(View? parent, string name, Context context, IAttributeSet attrs)
    {
        // resolve the tag name to a type
        var viewType = ViewTypeResolver?.Resolve(name);

        if (viewType == null)
        {
            //MvxBindingLog.Error( "View type not found - {0}", name);
            return null;
        }

        try
        {
            var view = ActivatorUtilities.CreateInstance(_serviceProvider, viewType, context, attrs) as View;
            if (view == null)
            {
                CrossBindingLog.Instance?.LogError("Unable to load view {ViewName} from type {ViewTypeName}",
                    name,
                    viewType.FullName);
            }
            return view;
        }
        catch (ThreadAbortException)
        {
            throw;
        }
        catch (Exception exception)
        {
            CrossBindingLog.Instance?.LogError(
                exception,
                "Exception during creation of {ViewName} from type {ViewTypeName}", name, viewType.FullName);
            return null;
        }
    }
}
