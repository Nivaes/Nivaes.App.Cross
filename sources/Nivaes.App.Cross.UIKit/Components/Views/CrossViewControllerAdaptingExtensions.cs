namespace Nivaes.App.Cross.UIKit
{
    public static class CrossViewControllerAdaptingExtensions
    {
        public static void AdaptForBinding(this ICrossEventSourceViewController view)
        {
            var adapter = new CrossViewControllerAdapter(view);
            var binding = new CrossBindingViewControllerAdapter(view);
        }
    }
}
