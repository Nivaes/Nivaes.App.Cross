namespace Nivaes.App.Cross.UIKit
{
    using Nivaes.App.Cross.Presenters;

    public class CrossPagePresentationAttribute : 
        CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
