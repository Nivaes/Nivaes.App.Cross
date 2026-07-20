using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI;

public static class CrossWindowsExtensions
{
    public static void OnViewCreate(this ICrossWindowsView storeView, byte[] requestBuffer, Func<ICrossBundle> bundleLoader)
    {
        storeView.OnViewCreate(() => { return storeView.LoadViewModel(requestBuffer, bundleLoader()); });
    }

    public static void OnViewCreate(this ICrossWindowsView storeView, Func<ICrossViewModel> viewModelLoader)
    {
        if (storeView.ViewModel != null)
            return;

        var viewModel = viewModelLoader();
        storeView.ViewModel = viewModel;
    }

    public static void OnViewDestroy(this ICrossWindowsView storeView, uint requestId)
    {
        //if (requestId > 0)
        //{
        //    Debugger.Break();
        //    //var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossWindowsViewModelRequestTranslator>();
        //    //viewModelLoader.RemoveSubViewModelWithKey(key);
        //}
        ViewModelRequestCache.Delete(requestId);
    }

    public static bool HasRegionAttribute(this Type view)
    {
        var attributes = view
            .GetCustomAttributes(typeof(RegionPresentationAttribute), true);

        return attributes.Any();
    }

    public static string GetRegionName(this Type view)
    {
        var attributes = view
            .GetCustomAttributes(typeof(RegionPresentationAttribute), true);

        if (!attributes.Any())
            throw new InvalidOperationException("The IMvxWindowsView has no region attribute.");

        return ((RegionPresentationAttribute)attributes.First()).Name;
    }

    public static T FindControl<T>(this UIElement parent, string name = null) where T : FrameworkElement
    {
        if (parent == null)
        {
            return null;
        }

        if (parent is T typedParent &&
            (string.IsNullOrWhiteSpace(name) || parent.GetValue(FrameworkElement.NameProperty).Equals(name)))
        {
            return typedParent;
        }

        T result = null;
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i) as UIElement;

            result = FindControl<T>(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return result;
    }

    private static ICrossViewModel LoadViewModel(this ICrossWindowsView storeView,
                                                byte[] requestBuffer,
                                                ICrossBundle bundle)
    {
        var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossWindowsViewModelLoader>();
        return viewModelLoader?.Load(requestBuffer, bundle);
    }
}
