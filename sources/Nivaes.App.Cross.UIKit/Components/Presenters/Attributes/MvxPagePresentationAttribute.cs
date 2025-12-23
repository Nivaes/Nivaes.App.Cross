namespace Nivaes.App.Cross.UIKit
{
    public class MvxPagePresentationAttribute
        : CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
