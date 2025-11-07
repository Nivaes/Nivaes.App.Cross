namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("No compatible con AoT", true)]
    public interface ICrossNamedInstanceRegistryFiller<out T>
    {
        string FindName(Type type);

        void FillFrom(
            ICrossNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type);

        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        void FillFrom(ICrossNamedInstanceRegistry<T> registry, Assembly assembly);
    }

    [Obsolete("No compatible con AoT", true)]
    public interface ICrossValueConverterRegistryFiller : ICrossNamedInstanceRegistryFiller<ICrossValueConverter>
    {
    }
}
