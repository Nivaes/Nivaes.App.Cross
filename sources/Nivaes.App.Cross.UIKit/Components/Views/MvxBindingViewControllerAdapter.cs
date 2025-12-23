namespace Nivaes.App.Cross.UIKit
{
    using System;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using MvvmCross.Logging;

    public class MvxBindingViewControllerAdapter 
        : MvxBaseViewControllerAdapter
    {
        protected IMvxIosView IosView => ViewController as IMvxIosView;

        public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
            : base(eventSource)
        {
            if (!(eventSource is IMvxIosView))
                throw new ArgumentException($"{nameof(eventSource)} should be a {nameof(IMvxIosView)}", nameof(eventSource));

            if (Mvx.IoCProvider?.TryResolve<IMvxBindingContext>(out var bindingContext) == true)
                IosView.BindingContext = bindingContext;
        }

        public override void HandleDisposeCalled(object sender, EventArgs e)
        {
            if (IosView == null)
            {
                MvxLogHost.GetLog<MvxBindingViewControllerAdapter>()?.LogWarning(
                    "{IosView} is null for clear-up of bindings", nameof(IosView));
                return;
            }
            IosView.ClearAllBindings();
            base.HandleDisposeCalled(sender, e);
        }
    }
}
