using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxUISegmentedControlSelectedSegmentTargetBinding(
            UISegmentedControl target,
            PropertyInfo targetPropertyInfo)
        : MvxPropertyInfoTargetBinding<UISegmentedControl>(target, targetPropertyInfo)
    {
        private CrossWeakEventSubscription<UISegmentedControl>? _subscription;

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var segmentedControl = View;
            if (segmentedControl == null)
            {
                CrossBindingLogger.Instance?.LogError(
                    "UISegmentedControl is null in MvxUISegmentedControlSelectedSegmentTargetBinding");
                return;
            }

            _subscription = segmentedControl.WeakSubscribe(nameof(segmentedControl.ValueChanged), HandleValueChanged);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is not UISegmentedControl view || value == null)
                return;

            view.SelectedSegment = (nint)value;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleValueChanged(object? sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged((int)view.SelectedSegment);
        }
    }
}