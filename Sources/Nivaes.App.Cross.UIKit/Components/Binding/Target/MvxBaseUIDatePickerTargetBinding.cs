#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Ios;

    public abstract class MvxBaseUIDatePickerTargetBinding(
        UIDatePicker target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UIDatePicker>(target, targetPropertyInfo)
    {
        private readonly NSTimeZone _systemTimeZone = NSTimeZone.SystemTimeZone;
        private CrossWeakEventSubscription<UIDatePicker>? _subscription;

        private void DatePickerOnValueChanged(object? sender, EventArgs args)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(GetValueFrom(view));
        }

        protected abstract object GetValueFrom(UIDatePicker view);

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var datePicker = View;
            if (datePicker == null)
            {
                CrossBindingLogger.Instance?.LogError("UIDatePicker is null in {TargetBindingType}",
                    nameof(MvxBaseUIDatePickerTargetBinding));
            }
            // Only listen for value changes if we are binding against one of the value-derived properties.
            else if (TargetPropertyInfo.Name is nameof(UIDatePicker.Date) or nameof(UIDatePicker.CountDownDuration))
            {
                _subscription = datePicker.WeakSubscribe(nameof(datePicker.ValueChanged), DatePickerOnValueChanged);
            }
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        protected DateTime ToLocalTime(DateTime utc)
        {
            if (utc.Kind == DateTimeKind.Local)
                return utc;

            var local = utc.AddSeconds(_systemTimeZone.SecondsFromGMT(utc.ToNSDate())).WithKind(DateTimeKind.Local);

            return local;
        }

        protected DateTime ToUtcTime(DateTime local)
        {
            if (local.Kind == DateTimeKind.Utc)
                return local;

            var utc = local.AddSeconds(-_systemTimeZone.SecondsFromGMT(local.ToNSDate())).WithKind(DateTimeKind.Utc);

            return utc;
        }
    }
}
#endif