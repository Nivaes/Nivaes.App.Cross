using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossNameCombinerManager : KeyContainerManager<ICrossValueCombiner>
{
    public CrossNameCombinerManager()
    {
    }

    public CrossNameCombinerManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(string viewName, [MaybeNullWhen(false)] out ICrossValueCombiner presentationType)
    {
        return base.TryGetValue(viewName.GetHashCode(), out presentationType);
    }
}
