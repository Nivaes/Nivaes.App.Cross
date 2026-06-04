using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Nivaes.App.Cross;
using Nivaes.IoC;

namespace Nivaes.App.Cross.WinUI;

public static class CrossWindowsExtensions
{
    public static void OnViewCreate(this ICrossWindowsView storeView, string requestText, Func<ICrossBundle> bundleLoader)
    {
        storeView.OnViewCreate(() => { return storeView.LoadViewModel(requestText, bundleLoader()); });
    }

    public static void OnViewCreate(this ICrossWindowsView storeView, Func<ICrossViewModel> viewModelLoader)
    {
        if (storeView.ViewModel != null)
            return;

        var viewModel = viewModelLoader();
        storeView.ViewModel = viewModel;
    }

    public static void OnViewDestroy(this ICrossWindowsView storeView, int key)
    {
        if (key > 0)
        {
            var viewModelLoader = Mvx.IoCProvider.Resolve<ICrossWindowsViewModelRequestTranslator>();
            viewModelLoader.RemoveSubViewModelWithKey(key);
        }
    }

    public static bool HasRegionAttribute(this Type view)
    {
        var attributes = view
            .GetCustomAttributes(typeof(MvxRegionPresentationAttribute), true);

        return attributes.Any();
    }

    public static string GetRegionName(this Type view)
    {
        var attributes = view
            .GetCustomAttributes(typeof(MvxRegionPresentationAttribute), true);

        if (!attributes.Any())
            throw new InvalidOperationException("The IMvxWindowsView has no region attribute.");

        return ((MvxRegionPresentationAttribute)attributes.First()).Name;
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
                                                string requestText,
                                                ICrossBundle bundle)
    {
        var viewModelLoader = Mvx.IoCProvider.Resolve<ICrossWindowsViewModelLoader>();
        return viewModelLoader?.Load(requestText, bundle);
    }
}
