namespace Nivaes.App.Cross.UIKitLib;

public class MvxModalPresentationAttribute : CrossBasePresentationAttribute
{
    public static readonly bool DefaultWrapInNavigationController = false;
    public static readonly UIModalPresentationStyle DefaultModalPresentationStyle = UIModalPresentationStyle.FullScreen;
    public static readonly UIModalTransitionStyle DefaultModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
    public static readonly CGSize DefaultPreferredContentSize = CGSize.Empty;
    public static readonly bool DefaultAnimated = true;

    public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

    public UIModalPresentationStyle ModalPresentationStyle { get; set; } = DefaultModalPresentationStyle;

    public UIModalTransitionStyle ModalTransitionStyle { get; set; } = DefaultModalTransitionStyle;

    public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

    public bool Animated { get; set; } = DefaultAnimated;
}