using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

[Obsolete("", true)]
public sealed class ValueKeyConvertersManager : KeyContainerManager<ICrossValueConverter>
{
    public ValueKeyConvertersManager()
    {
    }

    public ValueKeyConvertersManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public ICrossValueConverter GetValue(Type viewModelType, Type viewType)
    {
        var key = (viewModelType.GetType(), viewType.GetType()).GetHashCode();
        if (TryGetValue(viewModelType, viewType, out var autoValueConverter))
        {
            return autoValueConverter;
        }
        else
        {
            throw new AppException($"Unregistered {viewModelType.FullName} type of autoValueConverter.");
        }
    }

    public bool TryGetValue(Type viewModelType, Type viewType, [MaybeNullWhen(false)] out ICrossValueConverter combiner)
    {
        var key = (viewModelType.GetType(), viewType.GetType()).GetHashCode();
        return base.TryGetValue(key, out combiner);
    }
}
