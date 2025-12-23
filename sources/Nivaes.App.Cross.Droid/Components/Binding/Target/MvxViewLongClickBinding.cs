namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Android.Views;
    using MvvmCross.Binding;

    public class MvxViewLongClickBinding
    : MvxAndroidTargetBinding
    {
        private ICommand? _command;
        private CrossAndroidTargetEventSubscription<View, View.LongClickEventArgs>? _subscription;

        protected View? View => (View?)Target;

        public MvxViewLongClickBinding(View view)
            : base(view)
        {
            _subscription = view.DroidWeakSubscribe<View, View.LongClickEventArgs>(nameof(view.LongClick), ViewOnLongClick);
        }

        private void ViewOnLongClick(object? sender, View.LongClickEventArgs longClickEventArgs)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            _command = value as ICommand;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;

                _command = null;
            }
            base.Dispose(isDisposing);
        }
    }
}