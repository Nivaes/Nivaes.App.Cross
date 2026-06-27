using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitOS;

public class MvxBindingViewControllerAdapter
    : MvxBaseViewControllerAdapter
{
    protected IMvxIosView? IosView => ViewController as IMvxIosView;

    public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
        : base(eventSource)
    {
        if (!(eventSource is IMvxIosView))
            throw new ArgumentException($"{nameof(eventSource)} should be a {nameof(IMvxIosView)}", nameof(eventSource));

        var bindingContext = IPlatformApplication.Current!.Services.GetService<ICrossBindingContext>();
        if (bindingContext != null)
            IosView?.BindingContext = bindingContext;
    }

    public override void HandleDisposeCalled(object? sender, EventArgs e)
    {
        if (IosView == null)
        {
            var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxBindingViewControllerAdapter>>();
            logger?.LogWarning("{IosView} is null for clear-up of bindings", nameof(IosView));

            return;
        }
        IosView.ClearAllBindings();
        base.HandleDisposeCalled(sender, e);
    }
}
