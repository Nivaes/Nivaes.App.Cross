namespace Nivaes.App.Cross.UIKitLib;

public class MvxChildPresentationAttribute : CrossBasePresentationAttribute
{
    public static readonly bool DefaultAnimated = true;
    public bool Animated { get; set; } = DefaultAnimated;
}