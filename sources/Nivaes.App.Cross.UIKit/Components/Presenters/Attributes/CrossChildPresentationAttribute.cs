namespace Nivaes.App.Cross.UIKit
{
    using Nivaes.App.Cross.Presenters;

    public class CrossChildPresentationAttribute : 
        CrossBasePresentationAttribute
    {
        public static readonly bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
