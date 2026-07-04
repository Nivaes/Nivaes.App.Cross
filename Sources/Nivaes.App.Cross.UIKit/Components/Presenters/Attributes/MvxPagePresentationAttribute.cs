namespace Nivaes.App.Cross.UIKitLib;

public class MvxPagePresentationAttribute
    : CrossBasePresentationAttribute
{
    public static bool DefaultWrapInNavigationController = false;
    public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
}
