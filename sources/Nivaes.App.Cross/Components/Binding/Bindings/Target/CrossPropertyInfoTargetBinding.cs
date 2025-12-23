namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public abstract class CrossPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
        : CrossConvertingTargetBinding(target)
    {
        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // if the target property should be set to NULL on dispose then we clear it here
                // this is a fix for the possible memory leaks discussion started https://github.com/slodge/MvvmCross/issues/17#issuecomment-8527392
                var setToNullAttribute = TargetPropertyInfo.GetCustomAttribute<CrossSetToNullAfterBindingAttribute>(true);
                if (setToNullAttribute != null)
                {
                    SetValue(null);
                }
            }

            base.Dispose(isDisposing);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        [UnconditionalSuppressMessage("Trimming", "IL2073", Justification = "PropertyInfo.PropertyType doesn't preserve DynamicallyAccessedMembers annotations, but the property was obtained from a properly annotated source")]
        public override Type TargetValueType => TargetPropertyInfo.PropertyType;

        protected PropertyInfo TargetPropertyInfo { get; } = targetPropertyInfo;

        protected override void SetValueImpl(object target, object? value)
        {
            var setMethod = TargetPropertyInfo.GetSetMethod();
            setMethod?.Invoke(target, [value]);
        }
    }

    public abstract class MvxPropertyInfoTargetBinding<T>(
            object target, PropertyInfo targetPropertyInfo)
        : CrossPropertyInfoTargetBinding(target, targetPropertyInfo)
        where T : class
    {
        protected T? View => Target as T;
    }
}