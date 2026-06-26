using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross;

[Obsolete("", true)]
public class CrossNavigationSerializer
    : ICrossNavigationSerializer
{
    public CrossNavigationSerializer(ICrossTextSerializer serializer)
    {
        Serializer = serializer;
    }

    public ICrossTextSerializer Serializer { get; }
}

[Obsolete("", true)]
public class CrossNavigationSerializer<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>
        : CrossNavigationSerializer
            where T : class, ICrossTextSerializer
{
    public CrossNavigationSerializer()
        : base(IPlatformApplication.Current!.Services.GetRequiredService<T>())
    {
    }
}
