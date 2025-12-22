namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using MvvmCross.Binding;

    public class CrossEventNameTargetBinding<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] TTarget, TEventArgs>
        : CrossTargetBinding
            where TTarget : class
    {
        private readonly bool _useEventArgsAsCommandParameter;
        private readonly IDisposable _eventSubscription;

        private ICommand? _currentCommand;

        public CrossEventNameTargetBinding(TTarget target, string targetEventName, bool useEventArgsAsCommandParameter = true)
            : base(target)
        {
            _useEventArgsAsCommandParameter = useEventArgsAsCommandParameter;
            _eventSubscription = target.WeakSubscribe<TTarget, TEventArgs>(targetEventName, HandleEvent);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType { get; } = typeof(ICommand);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _eventSubscription?.Dispose();
            }
            base.Dispose(isDisposing);
        }

        private void HandleEvent(object? sender, TEventArgs parameter)
        {
            var commandParameter = _useEventArgsAsCommandParameter ? (object?)parameter : null;

            if (_currentCommand?.CanExecute(commandParameter) == true)
                _currentCommand.Execute(commandParameter);
        }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object? value) => _currentCommand = value as ICommand;
    }
}