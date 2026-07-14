using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class PresenterActionsKeyContainerManager : KeyContainerManager<IPressenterAction>
{
    public PresenterActionsKeyContainerManager()
    {
    }

    public PresenterActionsKeyContainerManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public IPressenterAction GetValue<TConverter>()
        where TConverter : ICrossValueConverter

    {
        return GetValue(typeof(TConverter));
    }

    public IPressenterAction GetValue(Type pressenterActionType)
    {
        if (TryGetValue(pressenterActionType, out var converter))
        {
            return converter;
        }
        else
        {
            throw new CrossException($"Unregistered {pressenterActionType.FullName} type of presenter action.");
        }
    }

    public bool TryGetValue(Type converterType, [MaybeNullWhen(false)] out IPressenterAction converter)
    {
        return base.TryGetValue(converterType.GetHashCode(), out converter);
    }
}
