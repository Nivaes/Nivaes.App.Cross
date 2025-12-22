namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxPagePresentationAttribute 
        : CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;

        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
