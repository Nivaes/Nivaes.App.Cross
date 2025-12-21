namespace MvvmCross.Binding.Binders
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Base;
    using MvvmCross.Converters;
    using Nivaes.App.Cross;

    public interface IMvxNamedInstanceRegistryFiller<out T>
    {
        string FindName(Type type);

        void FillFrom(
            ICrossNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type);

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        void FillFrom(ICrossNamedInstanceRegistry<T> registry, Assembly assembly);
    }

    public interface IMvxValueConverterRegistryFiller : IMvxNamedInstanceRegistryFiller<IMvxValueConverter>
    {
    }
}
