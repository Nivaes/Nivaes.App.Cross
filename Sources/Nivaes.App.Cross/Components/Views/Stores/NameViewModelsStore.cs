using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class NameViewModelsStore : KeyContainerManager<Type>
{
    public NameViewModelsStore()
    {
    }

    public NameViewModelsStore(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue(string viewName, [MaybeNullWhen(false)] out Type presentationType)
    {
        return base.TryGetValue(viewName.GetHashCode(), out presentationType);
    }
}
