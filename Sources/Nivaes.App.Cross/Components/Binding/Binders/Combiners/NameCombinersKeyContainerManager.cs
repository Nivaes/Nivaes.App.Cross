using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class NameCombinersKeyContainerManager : KeyContainerManager<ICrossValueCombiner>
{
    public NameCombinersKeyContainerManager()
    {
    }

    public NameCombinersKeyContainerManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(string viewName, [MaybeNullWhen(false)] out ICrossValueCombiner presentationType)
    {
        return base.TryGetValue(viewName.GetHashCode(), out presentationType);
    }
}
