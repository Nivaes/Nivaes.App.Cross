using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossCombinersManager : KeyContainerManager<ICrossValueCombiner>
{
    public CrossCombinersManager()
    {
    }

    public CrossCombinersManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(Type converterType, [MaybeNullWhen(false)] out ICrossValueCombiner presentationType)
    {
        return base.TryGetValue(converterType.GetHashCode(), out presentationType);
    }
}
