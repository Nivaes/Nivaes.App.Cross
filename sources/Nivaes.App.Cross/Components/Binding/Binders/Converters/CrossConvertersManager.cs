using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossConvertersManager : KeyContainerManager<ICrossValueConverter>
{
    public CrossConvertersManager()
    {
    }

    public CrossConvertersManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(Type converterType, [MaybeNullWhen(false)] out ICrossValueConverter presentationType)
    {
        return base.TryGetValue(converterType.GetHashCode(), out presentationType);
    }
}
