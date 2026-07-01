namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public sealed class CrossNullTargetBinding()
        : CrossTargetBinding(null)
    {
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneTime;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(object);

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object? value)
        {
            // ignored
        }
    }
}