namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;

    public interface ICrossTargetBinding
        : ICrossBinding
    {
        event EventHandler<CrossTargetChangedEventArgs>? ValueChanged;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        Type TargetValueType { get; }
        CrossBindingMode DefaultMode { get; }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        void SetValue(object? value);

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        void SubscribeToEvents();
    }
}