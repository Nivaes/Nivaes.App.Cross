namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    [Obsolete("No compatible con AoT")]
    public class CrossUIPageControlCurrentPageTargetBinding(UIPageControl target, PropertyInfo targetPropertyInfo)
        : CrossPropertyInfoTargetBinding<UIPageControl>(target, targetPropertyInfo)
    {
        private CrossWeakEventSubscription<UIPageControl>? _subscription;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is not UIPageControl view || value == null)
                return;

            view.CurrentPage = (nint)value;
        }

        private void HandleValueChanged(object? sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.CurrentPage);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var pageControl = View;
            if (pageControl == null)
            {
                CrossBindingLog.Instance?.LogError("UIPageControl is null in MvxUIPageControlCurrentPageTargetBinding");
                return;
            }

            _subscription = pageControl.WeakSubscribe(nameof(pageControl.ValueChanged), HandleValueChanged);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}