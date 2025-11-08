namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;

    public static class CrossWindowsPropertyBindingExtensions
    {
        public static string BindVisible(this FrameworkElement frameworkElement)
            => CrossWindowsPropertyBinding.FrameworkElement_Visible;

        public static string BindCollapsed(this FrameworkElement frameworkElement)
            => CrossWindowsPropertyBinding.FrameworkElement_Collapsed;

        public static string BindHidden(this FrameworkElement frameworkElement)
            => CrossWindowsPropertyBinding.FrameworkElement_Hidden;
    }
}
