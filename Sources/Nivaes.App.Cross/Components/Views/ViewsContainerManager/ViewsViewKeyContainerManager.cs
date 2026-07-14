using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class ViewsViewKeyContainerManager : KeyContainerManager<Type>
{
    public ViewsViewKeyContainerManager()
    {
    }

    public ViewsViewKeyContainerManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue<TView>([MaybeNullWhen(false)] out Type presentationType)
        where TView : class, ICrossView
    {
        return TryGetValue(typeof(TView), out presentationType);
    }

    public bool TryGetValue(Type viewModelType, [MaybeNullWhen(false)] out Type presentationType)
    {
        return base.TryGetValue(viewModelType.GetHashCode(), out presentationType);
    }
}
