namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public interface ICrossTypeToTypeLookupBuilder
    {
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        IDictionary<Type, Type> Build(IEnumerable<Assembly> sourceAssemblies);
    }
}
