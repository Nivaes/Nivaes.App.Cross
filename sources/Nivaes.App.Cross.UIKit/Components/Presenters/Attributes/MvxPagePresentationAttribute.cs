namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxPagePresentationAttribute
        : CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
