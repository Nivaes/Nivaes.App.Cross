using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKit
{
    public class CrossBindingViewControllerAdapter : CrossBaseViewControllerAdapter
    {
        protected ICrossIosView IosView => ViewController as ICrossIosView;

        public CrossBindingViewControllerAdapter(ICrossEventSourceViewController eventSource)
            : base(eventSource)
        {
            throw new NotImplementedException();

            //if (!(eventSource is ICrossIosView))
            //    throw new ArgumentException($"{nameof(eventSource)} should be a {nameof(ICrossIosView)}", nameof(eventSource));

            //if (Mvx.IoCProvider?.TryResolve<ICrossBindingContext>(out var bindingContext) == true)
            //    IosView.BindingContext = bindingContext;
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (IosView == null)
            {
                CrossLogHost.GetLog<CrossBindingViewControllerAdapter>()?.LogWarning(
                    "{IosView} is null for clear-up of bindings", nameof(IosView));
                return;
            }
            IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
