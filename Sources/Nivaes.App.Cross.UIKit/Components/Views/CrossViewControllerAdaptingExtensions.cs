namespace Nivaes.App.Cross.UIKitOS
{
    public static class CrossViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            new MvxViewControllerAdapter(view);
            new MvxBindingViewControllerAdapter(view);
        }
    }
}
