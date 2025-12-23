namespace Nivaes.App.Cross.AppKit
{
    public static class MvxViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            new MvxViewControllerAdapter(view);
            new MvxBindingViewControllerAdapter(view);
        }
    }
}
