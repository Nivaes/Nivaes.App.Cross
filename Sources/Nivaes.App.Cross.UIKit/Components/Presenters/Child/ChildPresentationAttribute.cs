namespace Nivaes.App.Cross.UIKitLib;

public class ChildPresentationAttribute : BasePresentationAttribute
{
    public static readonly bool DefaultAnimated = true;
    public bool Animated { get; set; } = DefaultAnimated;
}