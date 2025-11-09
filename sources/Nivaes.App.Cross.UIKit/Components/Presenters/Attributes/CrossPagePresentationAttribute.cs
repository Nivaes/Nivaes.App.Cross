namespace Nivaes.App.Cross.UIKit
{
    public class CrossPagePresentationAttribute : 
        CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
