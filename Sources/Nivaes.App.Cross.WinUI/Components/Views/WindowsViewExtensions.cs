using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI;

public static class WindowsViewExtensions
{
    private static ICrossWindowsViewModelLoader? ViewModelLoader;
    public static void OnViewCreate(this ICrossWindowsView storeView, byte[] requestBuffer, Func<ICrossBundle> bundleLoader)
    {
        storeView.OnViewCreate(() => 
        {
            if(ViewModelLoader == null)
                ViewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ICrossWindowsViewModelLoader>();

            return ViewModelLoader.Load(requestBuffer, bundleLoader());
        });
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
        ViewModelRequestCache.Delete(requestId);
    }

    //public static bool HasRegionAttribute(this Type view)
    //{
    //    var attributes = view
    //        .GetCustomAttributes(typeof(RegionPresentationAttribute), true);

    //    return attributes.Any();
    //}

    //public static string? GetRegionName(this Type view)
    //{
    //    var attributes = view.GetCustomAttributes(typeof(RegionPresentationAttribute), true);

    //    if (!attributes.Any())
    //        throw new AppException($"The {view.FullName} has no region attribute.");

    //    return ((RegionPresentationAttribute)attributes.First()).RegionName;
    //}

    public static T? FindControl<T>(this UIElement parent, string? name = null) where T : FrameworkElement
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

        T? result = null;
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            var child = (UIElement)VisualTreeHelper.GetChild(parent, i);

            result = FindControl<T>(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return result;
    }
}
