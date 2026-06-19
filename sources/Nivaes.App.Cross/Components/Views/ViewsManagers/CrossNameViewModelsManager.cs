using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossNameViewModelsManager : KeyContainerManager<Type>
{
    public CrossNameViewModelsManager()
    {
    }

    public CrossNameViewModelsManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(string viewName, [MaybeNullWhen(false)] out Type presentationType)
    {
        return base.TryGetValue(viewName.GetHashCode(), out presentationType);
    }
}
