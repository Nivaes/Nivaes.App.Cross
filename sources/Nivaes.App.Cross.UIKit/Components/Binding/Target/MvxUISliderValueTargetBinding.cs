#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUISliderValueTargetBinding(
        UISlider target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UISlider>(target, targetPropertyInfo)
    {
        private CrossWeakEventSubscription<UISlider>? _subscription;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is not UISlider view || value == null)
                return;

            view.Value = (float)value;
        }

        private void HandleSliderValueChanged(object? sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var slider = View;
            if (slider == null)
            {
                CrossBindingLogger.Instance?.LogError("UISlider is null in MvxUISliderValueTargetBinding");
                return;
            }

            _subscription = slider.WeakSubscribe(nameof(slider.ValueChanged), HandleSliderValueChanged);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing)
                return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
#endif