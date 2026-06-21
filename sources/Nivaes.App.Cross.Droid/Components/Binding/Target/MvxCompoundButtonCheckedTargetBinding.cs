namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;

    public class MvxCompoundButtonCheckedTargetBinding(
        object target,
        PropertyInfo targetPropertyInfo)
    : MvxAndroidPropertyInfoTargetBinding<CompoundButton>(target, targetPropertyInfo)
    {
        private CrossAndroidTargetEventSubscription<CompoundButton, CompoundButton.CheckedChangeEventArgs>? _subscription;

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var compoundButton = View;
            if (compoundButton == null)
            {
                CrossBindingLogger.Instance?.LogError(
                    "compoundButton is null in MvxCompoundButtonCheckedTargetBinding");
                return;
            }

            _subscription = compoundButton.DroidWeakSubscribe<CompoundButton, CompoundButton.CheckedChangeEventArgs>(
                nameof(compoundButton.CheckedChange),
                CompoundButtonOnCheckedChange);
        }

        private void CompoundButtonOnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs args)
        {
            FireValueChanged(View?.Checked);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }
    }
}