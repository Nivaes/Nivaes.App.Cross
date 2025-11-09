namespace Nivaes.App.Cross.UIKit
{
    public class CrossTabPresentationAttribute : 
        CrossBasePresentationAttribute
    {
        public string TabName { get; set; }

        public string TabIconName { get; set; }

        public string TabSelectedIconName { get; set; }

        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public string TabAccessibilityIdentifier { get; set; }
    }
}