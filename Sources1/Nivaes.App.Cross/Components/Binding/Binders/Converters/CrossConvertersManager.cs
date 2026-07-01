using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class CrossConvertersManager : KeyContainerManager<ICrossValueConverter>
{
    public CrossConvertersManager()
    {
    }

    public CrossConvertersManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public ICrossValueConverter GetValue<TConverter>()
        where TConverter : ICrossValueConverter

    {
        return GetValue(typeof(TConverter));
    }

    public ICrossValueConverter GetValue(Type converterType)
    {
        if (TryGetValue(converterType, out var converter))
        {
            return converter;
        }
        else
        {
            throw new CrossException($"Unregistered {converterType.FullName} type of converter.");
        }
    }

    public bool TryGetValue(Type converterType, [MaybeNullWhen(false)] out ICrossValueConverter converter)
    {
        return base.TryGetValue(converterType.GetHashCode(), out converter);
    }
}
