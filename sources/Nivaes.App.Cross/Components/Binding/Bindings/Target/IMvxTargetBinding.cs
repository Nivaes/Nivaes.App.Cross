namespace MvvmCross.Binding.Bindings.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public interface IMvxTargetBinding
        : ICrossBinding
    {
        event EventHandler<MvxTargetChangedEventArgs>? ValueChanged;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        Type TargetValueType { get; }
        MvxBindingMode DefaultMode { get; }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        void SetValue(object? value);

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        void SubscribeToEvents();
    }
}