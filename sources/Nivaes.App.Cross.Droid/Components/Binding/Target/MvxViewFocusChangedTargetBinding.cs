using Android.Views;
using static Android.Views.View;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Android.Views;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxViewFocusChangedTargetBinding 
        : MvxAndroidTargetBinding
    {
        private ICommand? _command;
        private CrossWeakEventSubscription<View, FocusChangeEventArgs>? _focusChangeSubscription;

        public MvxViewFocusChangedTargetBinding(View target) : base(target)
        {
            _focusChangeSubscription = target.WeakSubscribe<View, FocusChangeEventArgs>(
                nameof(target.FocusChange), ViewOnFocusChange);
        }

        private void ViewOnFocusChange(object? sender, FocusChangeEventArgs e)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(e.HasFocus))
                return;

            _command.Execute(e.HasFocus);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            _command = value as ICommand;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _focusChangeSubscription?.Dispose();
                _focusChangeSubscription = null;
            }

            base.Dispose(isDisposing);
        }
    }
}