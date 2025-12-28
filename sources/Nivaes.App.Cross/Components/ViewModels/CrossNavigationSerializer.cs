using System.Diagnostics.CodeAnalysis;
using Nivaes.IoC;

namespace Nivaes.App.Cross;

public class CrossNavigationSerializer
    : ICrossNavigationSerializer
{
    public CrossNavigationSerializer(ICrossTextSerializer serializer)
    {
        Serializer = serializer;
    }

    public ICrossTextSerializer Serializer { get; }
}

public class CrossNavigationSerializer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>
        : CrossNavigationSerializer
            where T : class, ICrossTextSerializer
{
    public CrossNavigationSerializer()
        : base(Mvx.IoCProvider.Resolve<T>())
    {
    }
}
