using System;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS;

public class MvxBindingViewControllerAdapter
    : MvxBaseViewControllerAdapter
{
    protected IMvxMacView? MacView
    {
        get { return ViewController as IMvxMacView; }
    }

    public MvxBindingViewControllerAdapter(IMvxEventSourceViewController eventSource)
        : base(eventSource)
    {
        if (!(eventSource is IMvxMacView))
            throw new ArgumentException(nameof(eventSource), $"{nameof(eventSource)} should be a {nameof(IMvxMacView)}");

        MacView.BindingContext = new CrossBindingContext();
    }

    public override void HandleDisposeCalled(object sender, EventArgs e)
    {
        if (MacView == null)
        {
            CrossLoggerHost.Default?.Log(LogLevel.Warning, "{PropertyName} is null for clearup of bindings", nameof(MacView));
            return;
        }
        MacView.ClearAllBindings();
        base.HandleDisposeCalled(sender, e);
    }
}
