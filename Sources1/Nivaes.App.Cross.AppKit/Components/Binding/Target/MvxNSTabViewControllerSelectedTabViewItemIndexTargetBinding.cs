namespace Nivaes.App.Cross.AppKitOS
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding
        : MvxPropertyInfoTargetBinding<NSTabViewController>
    {
        private bool _subscribed;

        public MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged((int)view.SelectedTabViewItemIndex);
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.TwoWay; }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var view = View;
            if (view == null)
            {
                CrossBindingLogger.Instance?.LogError("NSTabViewController is null in MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding");
                return;
            }

            _subscribed = true;
            if (view is MvxEventSourceTabViewController)
            {
                ((MvxEventSourceTabViewController)view).DidSelectCalled += HandleValueChanged;
            }
            else
            {
                try
                {
                    view.TabView.DidSelect += HandleValueChanged;
                }
                catch (Exception ex)
                {
                    CrossBindingLogger.Instance?.LogError(ex, "Failed to subscribe to events");
                }
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as NSTabViewController;
            if (view == null)
                return;

            view.SelectedTabViewItemIndex = (int)value;
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var view = View;
                if (view != null && _subscribed)
                {
                    if (view is MvxEventSourceTabViewController)
                    {
                        ((MvxEventSourceTabViewController)view).DidSelectCalled -= HandleValueChanged;
                    }
                    else
                    {
                        try
                        {
                            view.TabView.DidSelect -= HandleValueChanged;
                        }
                        catch (Exception ex)
                        {
                            CrossBindingLogger.Instance?.LogError(ex, "Failed to unsubscribe from event");
                        }
                    }
                    _subscribed = false;
                }
            }
        }
    }
}
