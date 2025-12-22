namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxSeekBarProgressTargetBinding
    : MvxPropertyInfoTargetBinding<SeekBar>
    {
        private CrossWeakEventSubscription<SeekBar, SeekBar.ProgressChangedEventArgs>? _subscription;

        public MvxSeekBarProgressTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var seekbar = (SeekBar?)target;
            if (seekbar == null || value == null)
                return;

            seekbar.Progress = (int)value;
        }

        private void SeekBarProgressChanged(object? sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.FromUser)
                FireValueChanged(e.Progress);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var seekBar = View;
            if (seekBar == null)
            {
                MvxBindingLog.Instance?.LogError("SeekBar is null in MvxSeekBarProgressTargetBinding");
                return;
            }

            _subscription = seekBar.WeakSubscribe<SeekBar, SeekBar.ProgressChangedEventArgs>(
                nameof(seekBar.ProgressChanged),
                SeekBarProgressChanged);
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