namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using AndroidX.Preference;
    using Microsoft.Extensions.Logging;

    public class CrossPreferenceValueTargetBinding(Preference preference)
        : CrossAndroidTargetBinding(preference)
    {
        private CrossAndroidTargetEventSubscription<Preference, Preference.PreferenceChangeEventArgs>? _subscription;

        public Preference? Preference => Target as Preference;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(Preference);

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            _subscription = Preference?.WeakSubscribe<Preference, Preference.PreferenceChangeEventArgs>(
                nameof(Preference.PreferenceChange),
                HandlePreferenceChange);
        }

        protected void HandlePreferenceChange(object? sender, Preference.PreferenceChangeEventArgs e)
        {
            if (e.Preference != Preference)
                return;

            FireValueChanged(e.NewValue);
            e.Handled = true;
        }

        [RequiresUnreferencedCode("This method calls SubscribeToEvents which may use reflection to subscribe to events which may not be preserved by trimming")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            CrossBindingLog.Instance?.LogWarning("SetValueImpl called on generic Preference target");
        }
    }
}