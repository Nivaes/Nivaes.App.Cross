namespace Nivaes.App.Cross.UIKitOS
{
    public static class MvxViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceViewController view)
        {
            var adapter = new MvxViewControllerAdapter(view);
            var binding = new MvxBindingViewControllerAdapter(view);
        }
    }
}
