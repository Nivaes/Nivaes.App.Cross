using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Nivaes.App.Cross.Components;

public static class CrossTypeExtensions
{
    public static object? CreateDefault(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] this Type? type)
    {
        if (type == null)
            return null;

        if (!type.GetTypeInfo().IsValueType)
        {
            return null;
        }

        if (Nullable.GetUnderlyingType(type) != null)
            return null;

        return Activator.CreateInstance(type);
    }
}
