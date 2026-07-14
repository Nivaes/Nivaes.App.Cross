using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class ViewModelViewsStore : KeyContainerManager<Type>
{
    public ViewModelViewsStore()
    {
    }

    public ViewModelViewsStore(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public bool TryGetValue<TViewModel>([MaybeNullWhen(false)] out Type presentationType)
        where TViewModel : class, ICrossViewModel
    {
        return TryGetValue(typeof(TViewModel), out presentationType);
    }

    public bool TryGetValue(Type viewModelType, [MaybeNullWhen(false)] out Type presentationType)
    {
        return base.TryGetValue(viewModelType.GetHashCode(), out presentationType);
    }
}
