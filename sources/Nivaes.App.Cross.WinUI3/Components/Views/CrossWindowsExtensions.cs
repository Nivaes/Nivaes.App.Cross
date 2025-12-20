namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.Extensions.Logging;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;
    using Nivaes.IoC;

    public static class CrossWindowsExtensions
    {
        public static void OnViewCreate(this ICrossWindowsView storeView, string requestText, Func<ICrossBundle?> bundleLoader)
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
                throw new NotImplementedException();
                //var viewModelLoader = Cross.IoCProvider.Resolve<ICrossWindowsViewModelRequestTranslator>();
                //viewModelLoader.RemoveSubViewModelWithKey(key);
            }
        }

        public static bool HasRegionAttribute(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(CrossRegionPresentationAttribute), true);

            return attributes.Any();
        }

        public static string GetRegionName(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(CrossRegionPresentationAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException("The ICrossWindowsView has no region attribute.");

            return ((CrossRegionPresentationAttribute)attributes.First()).Name;
        }

        public static T? FindControl<T>(this UIElement? parent, string? name = null) where T : FrameworkElement
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
                var child = VisualTreeHelper.GetChild(parent, i) as UIElement;

                result = child?.FindControl<T>(name);
                if (result != null)
                {
                    return result;
                }
            }

            return result;
        }

        private static ICrossViewModel LoadViewModel(this ICrossWindowsView storeView,
                                                    string requestText,
                                                    ICrossBundle? bundle)
        {
            var container = Nivaes.Singleton<CrossIoCServiceContainer>.Instance;
            var viewModelLoader = container.Resolve<ICrossWindowsViewModelLoader>();
            return viewModelLoader!.Load(requestText, bundle);
        }
    }
}
