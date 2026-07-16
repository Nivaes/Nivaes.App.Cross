using Nivaes.App.Cross;

namespace Nivaes.App.Cross.UIKitLib
{
    public class TabPresentationAttribute 
        : BasePresentationAttribute
    {
        public string? TabName { get; set; }

        public string? TabIconName { get; set; }

        public string? TabSelectedIconName { get; set; }

        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public string? TabAccessibilityIdentifier { get; set; }
    }
}
