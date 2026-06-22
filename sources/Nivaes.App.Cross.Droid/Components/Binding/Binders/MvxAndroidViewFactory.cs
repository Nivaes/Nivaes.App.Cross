using Android.Content;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public class MvxAndroidViewFactory
    : IMvxAndroidViewFactory
{
    private IMvxViewTypeResolver _viewTypeResolver;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public MvxAndroidViewFactory(IServiceProvider serviceProvider, IMvxViewTypeResolver viewTypeResolver,
        ILogger<MvxAndroidViewFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _viewTypeResolver = viewTypeResolver;
        _logger = logger;
    }

    public virtual View? CreateView(View? parent, string name, Context context, IAttributeSet attrs)
    {
        // resolve the tag name to a type
        var viewType = _viewTypeResolver.Resolve(name);

        if (viewType == null)
        {
            return null;
        }

        try
        {
            var view = ActivatorUtilities.CreateInstance(_serviceProvider, viewType, context, attrs) as View;
            if (view == null)
            {
                _logger.LogError("Unable to load view {ViewName} from type {ViewTypeName}",
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
            _logger.LogError(
                exception,
                "Exception during creation of {ViewName} from type {ViewTypeName}", name, viewType.FullName);
            return null;
        }
    }
}
