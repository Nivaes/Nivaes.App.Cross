namespace Nivaes.App.Cross.UIKitOS;

public class MvxChildPresentationAttribute : CrossBasePresentationAttribute
{
    public static readonly bool DefaultAnimated = true;
    public bool Animated { get; set; } = DefaultAnimated;
}