namespace Nivaes.App.Cross.UIKit
{
    public class CrossChildPresentationAttribute : 
        CrossBasePresentationAttribute
    {
        public static readonly bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
