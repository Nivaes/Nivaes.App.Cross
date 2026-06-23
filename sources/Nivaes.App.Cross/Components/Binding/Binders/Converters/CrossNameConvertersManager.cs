using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossNameConvertersManager : KeyContainerManager<ICrossValueConverter>
{
    public CrossNameConvertersManager()
    {
    }

    public CrossNameConvertersManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(string viewName, [MaybeNullWhen(false)] out ICrossValueConverter presentationType)
    {
        return base.TryGetValue(viewName.GetHashCode(), out presentationType);
    }
}
