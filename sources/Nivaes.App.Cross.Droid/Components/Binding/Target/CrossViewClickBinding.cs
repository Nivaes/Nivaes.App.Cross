namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Android.Views;

    public class CrossViewClickBinding
        : CrossAndroidTargetBinding
    {
        private ICommand? _command;

        private readonly EventHandler<EventArgs> _canExecuteEventHandler;
        private CrossWeakEventSubscription<View>? _clickSubscription;
        private CrossCanExecuteChangedEventSubscription? _canExecuteSubscription;

        protected View? View => (View?)Target;

        public CrossViewClickBinding(View view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;
            _clickSubscription = view.WeakSubscribe(nameof(view.Click), ViewOnClick);
        }

        private void ViewOnClick(object? sender, EventArgs args)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;

            _command = value as ICommand;
            if (_command != null)
            {
                _canExecuteSubscription = _command.WeakSubscribe(_canExecuteEventHandler);
            }
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = View;
            if (view == null)
                return;

            var shouldBeEnabled = false;
            if (_command != null)
            {
                shouldBeEnabled = _command.CanExecute(null);
            }
            view.Enabled = shouldBeEnabled;
        }

        private void OnCanExecuteChanged(object? sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _clickSubscription?.Dispose();
                _clickSubscription = null;

                _canExecuteSubscription?.Dispose();
                _canExecuteSubscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}