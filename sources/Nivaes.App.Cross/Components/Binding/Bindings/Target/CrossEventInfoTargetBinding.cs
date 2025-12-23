namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Windows.Input;
    using MvvmCross.Binding;

    public class CrossEventInfoTargetBinding<T> : CrossTargetBinding
        where T : EventArgs
    {
        private readonly EventInfo _targetEventInfo;

        private ICommand? _currentCommand;

        public CrossEventInfoTargetBinding(object target,
            EventInfo targetEventInfo)
            : base(target)
        {
            _targetEventInfo = targetEventInfo;

            // 	addMethod is used because of error:
            // "Attempting to JIT compile method '(wrapper delegate-invoke) <Module>:invoke_void__this___UIControl_EventHandler (UIKit.UIControl,System.EventHandler)' while running with --aot-only."
            // see https://bugzilla.xamarin.com/show_bug.cgi?id=3682
            var addMethod = _targetEventInfo.GetAddMethod();
            addMethod?.Invoke(target, [new EventHandler<T>(HandleEvent)]);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    _targetEventInfo.GetRemoveMethod()?.Invoke(target, [new EventHandler<T>(HandleEvent)]);
                }
            }

            base.Dispose(isDisposing);
        }

        private void HandleEvent(object? sender, T args)
        {
            _currentCommand?.Execute(null);
        }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object? value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }
    }
}