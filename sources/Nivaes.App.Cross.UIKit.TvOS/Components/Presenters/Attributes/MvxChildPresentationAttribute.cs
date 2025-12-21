namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxChildPresentationAttribute 
        : CrossBasePresentationAttribute
    {
        public static bool DefaultAnimated = true;

        public bool Animated { get; set; } = DefaultAnimated;
    }
}
