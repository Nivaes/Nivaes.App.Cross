using AndroidX.Preference;

namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using Nivaes.App.Cross;

    public class MvxPreferenceClickTargetBinding
        : MvxAndroidTargetBinding
    {
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;
        private ICommand? _command;
        private CrossAndroidTargetEventSubscription<Preference, Preference.PreferenceClickEventArgs>? _clickSubscription;
        private CrossCanExecuteChangedEventSubscription? _canExecuteSubscription;

        protected Preference? Preference => (Preference?)Target;

        public MvxPreferenceClickTargetBinding(Preference view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;

            _clickSubscription = CrossAndroidWeakSubscriptionExtensions.DroidWeakSubscribe<Preference, Preference.PreferenceClickEventArgs>(view, nameof(Preference.PreferenceClick),
                ViewOnPreferenceClick);
        }

        private void ViewOnPreferenceClick(object? sender, Preference.PreferenceClickEventArgs args)
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
            var view = Preference;
            if (view == null)
                return;

            view.Enabled = _command?.CanExecute(null) ?? false;
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