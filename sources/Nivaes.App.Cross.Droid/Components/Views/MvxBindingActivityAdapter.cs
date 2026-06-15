using System;

namespace Nivaes.App.Cross.Droid;

public class MvxBindingActivityAdapter
    : MvxBaseActivityAdapter
{
    private IMvxAndroidBindingContext? BindingContext
    {
        get
        {
            var contextOwner = (ICrossBindingContextOwner?)Activity;
            return (IMvxAndroidBindingContext?)contextOwner?.BindingContext;
        }
    }

    public MvxBindingActivityAdapter(ICrossEventSourceActivity eventSource)
        : base(eventSource)
    {
    }

    protected override void EventSourceOnCreateWillBeCalled(object? sender,
                                                            CrossValueEventArgs<Bundle> MvxValueEventArgs)
    {
        BindingContext?.ClearAllBindings();
        base.EventSourceOnCreateWillBeCalled(sender, MvxValueEventArgs);
    }

    protected override void EventSourceOnDestroyCalled(object? sender, EventArgs eventArgs)
    {
        BindingContext?.ClearAllBindings();
        base.EventSourceOnDestroyCalled(sender, eventArgs);
    }

    protected override void EventSourceOnDisposeCalled(object? sender, EventArgs eventArgs)
    {
        BindingContext?.ClearAllBindings();
        base.EventSourceOnDisposeCalled(sender, eventArgs);
    }
}
