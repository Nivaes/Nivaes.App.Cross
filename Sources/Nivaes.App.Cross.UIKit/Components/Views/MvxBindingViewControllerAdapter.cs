using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.UIKitLib;

public class MvxBindingViewControllerAdapter
    : MvxBaseViewControllerAdapter
{
    protected IMvxIosView? IosView => ViewController as IMvxIosView;

    public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
        : base(eventSource)
    {
        if (!(eventSource is IMvxIosView))
            throw new ArgumentException($"{nameof(eventSource)} should be a {nameof(IMvxIosView)}", nameof(eventSource));

        var bindingContext = IPlatformApplication.Current!.ServiceProvider.GetService<ICrossBindingContext>();
        if (bindingContext != null)
            IosView?.BindingContext = bindingContext;
    }

    public override void HandleDisposeCalled(object? sender, EventArgs e)
    {
        if (IosView == null)
        {
            CrossLoggerHost.GetLogger<MvxBindingViewControllerAdapter>().LogWarning("{IosView} is null for clear-up of bindings", nameof(IosView));

            return;
        }
        IosView.ClearAllBindings();
        base.HandleDisposeCalled(sender, e);
    }
}
