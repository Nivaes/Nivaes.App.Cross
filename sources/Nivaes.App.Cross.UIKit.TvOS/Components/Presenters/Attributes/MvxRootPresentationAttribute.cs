namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    using MvvmCross.Presenters.Attributes;

    public class MvxRootPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
