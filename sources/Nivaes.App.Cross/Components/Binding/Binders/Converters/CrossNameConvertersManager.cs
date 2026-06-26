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

    public ICrossValueConverter GetValue(string converterName)
    {
        if (TryGetValue(converterName, out var converter))
        {
            return converter;
        }
        else
        {
            throw new CrossException($"Unregistered {converterName} type of converter.");
        }
    }

    public bool TryGetValue(string converterName, [MaybeNullWhen(false)] out ICrossValueConverter converter)
    {
        return base.TryGetValue(converterName.GetHashCode(), out converter);
    }
}
