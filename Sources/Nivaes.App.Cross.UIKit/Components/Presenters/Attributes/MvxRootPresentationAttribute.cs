namespace Nivaes.App.Cross.UIKitLib;

public class MvxRootPresentationAttribute
    : CrossBasePresentationAttribute
{
    public static readonly float DefaultAnimationDuration = 1.0f;

    public static readonly bool DefaultWrapInNavigationController = false;

    public static readonly UIViewAnimationOptions DefaultAnimationOptions = UIViewAnimationOptions.TransitionNone;

    public float AnimationDuration { get; set; } = DefaultAnimationDuration;

    public UIViewAnimationOptions AnimationOptions { get; set; } = DefaultAnimationOptions;

    public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
}