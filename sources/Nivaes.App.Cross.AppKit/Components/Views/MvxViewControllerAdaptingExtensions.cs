namespace Nivaes.App.Cross.AppKitOS
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
