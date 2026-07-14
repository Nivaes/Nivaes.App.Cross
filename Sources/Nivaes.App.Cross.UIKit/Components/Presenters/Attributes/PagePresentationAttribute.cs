namespace Nivaes.App.Cross.UIKitLib;

public class PagePresentationAttribute
    : BasePresentationAttribute
{
    public static bool DefaultWrapInNavigationController = false;
    public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
}
