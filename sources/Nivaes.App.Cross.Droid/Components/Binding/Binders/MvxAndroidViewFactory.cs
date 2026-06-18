using Android.Content;
using Android.Util;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid;

public class MvxAndroidViewFactory
    : IMvxAndroidViewFactory
{
    private IMvxViewTypeResolver? _viewTypeResolver;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    // ToDo: Solucionar la referencia circular. Probablemente fusionando las dos clases.
    protected IMvxViewTypeResolver ViewTypeResolver => _viewTypeResolver ??= IPlatformApplication.Current!.Services.GetRequiredService<IMvxViewTypeResolver>();

    public MvxAndroidViewFactory(IServiceProvider serviceProvider,
        ILogger<MvxAndroidViewFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public virtual View? CreateView(View? parent, string name, Context context, IAttributeSet attrs)
    {
        // resolve the tag name to a type
        var viewType = ViewTypeResolver.Resolve(name);

        if (viewType == null)
        {
            _logger.LogError("View type not found - {0}", name);
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
