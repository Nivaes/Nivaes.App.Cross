namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public abstract class CrossTargetBinding 
        : CrossBinding, ICrossTargetBinding
    {
        public event EventHandler<CrossTargetChangedEventArgs>? ValueChanged;

        private readonly WeakReference _target;

        protected CrossTargetBinding(object? target)
        {
            _target = new WeakReference(target);
        }

        protected object? Target => _target.Target;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public virtual void SubscribeToEvents()
        {
            // do nothing by default
        }

        protected virtual void FireValueChanged(object? newValue)
        {
            ValueChanged?.Invoke(this, new CrossTargetChangedEventArgs(newValue));
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public abstract Type TargetValueType { get; }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public abstract void SetValue(object? value);

        public abstract CrossBindingMode DefaultMode { get; }
    }

    public abstract class MvxTargetBinding<
            TTarget,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue
        > : CrossBinding, ICrossTargetBinding
        where TTarget : class
    {
        public event EventHandler<CrossTargetChangedEventArgs>? ValueChanged;

        private readonly WeakReference<TTarget> _target;

        protected MvxTargetBinding(TTarget target)
        {
            _target = new WeakReference<TTarget>(target);
        }

        protected TTarget? Target
        {
            get
            {
                _target.TryGetTarget(out var target);
                return target;
            }
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public virtual void SubscribeToEvents()
        {
            // do nothing by default
        }

        protected virtual void FireValueChanged(TValue? newValue)
        {
            ValueChanged?.Invoke(this, new CrossTargetChangedEventArgs(newValue));
        }

        public abstract CrossBindingMode DefaultMode { get; }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public Type TargetValueType => typeof(TValue);

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected abstract void SetValue(TValue? value);

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public void SetValue(object? value)
        {
            if (value != null && value is not TValue)
            {
                CrossBindingLogger.Instance?.LogError(
                    "Invalid value type for target binding {TypeName}: received {ValueTypeName} but expects {ExpectedTypeName}, and cast failed",
                    GetType().Name, value.GetType().Name, typeof(TValue).Name);
                return;
            }

            SetValue(value == null ? default : (TValue)value);
        }
    }
}