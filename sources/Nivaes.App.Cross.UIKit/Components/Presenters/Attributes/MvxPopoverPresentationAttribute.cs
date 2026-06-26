namespace MvvmCross.Platforms.Ios.Presenters.Attributes
{
    using Nivaes.App.Cross;


    public class MvxPopoverPresentationAttribute
        : CrossBasePresentationAttribute
    {
        public static readonly bool DefaultWrapInNavigationController = false;
        public static readonly CGSize DefaultPreferredContentSize = CGSize.Empty;
        public static readonly bool DefaultAnimated = true;
        public static readonly UIPopoverArrowDirection DefaultPermittedArrowDirections = UIPopoverArrowDirection.Any;

        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

        public bool Animated { get; set; } = DefaultAnimated;

        public UIPopoverArrowDirection PermittedArrowDirections { get; set; } = DefaultPermittedArrowDirections;
    }
}