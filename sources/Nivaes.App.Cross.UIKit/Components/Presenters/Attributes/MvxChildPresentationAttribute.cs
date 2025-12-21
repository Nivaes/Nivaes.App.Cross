namespace MvvmCross.Platforms.Ios.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxChildPresentationAttribute : CrossBasePresentationAttribute
    {
        public static readonly bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}