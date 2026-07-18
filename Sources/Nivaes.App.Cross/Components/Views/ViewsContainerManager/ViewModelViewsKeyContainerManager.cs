using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class ViewModelViewsKeyContainerManager : KeyContainerManager<Type>
{
    public ViewModelViewsKeyContainerManager()
    {
    }

    public ViewModelViewsKeyContainerManager(KeyStoreItem[] presentations)
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
        return base.TryGetValue(viewModelType.TypeHandle.Value, out presentationType);
    }

    public Type GetValue(Type viewModelType)
    {
        if (!TryGetValue(viewModelType, out var viewType))
        {
            throw new AppException($"Not fount ViewType asociate to {viewModelType.FullName}.");
        } 
        
        return viewType;
    }
}
