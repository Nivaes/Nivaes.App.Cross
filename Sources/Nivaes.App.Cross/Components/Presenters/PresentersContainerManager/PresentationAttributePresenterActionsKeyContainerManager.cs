using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public sealed class PresentationAttributePresenterActionsKeyContainerManager : KeyContainerManager<IPressenterAction>
{
    public PresentationAttributePresenterActionsKeyContainerManager()
    {
    }

    public PresentationAttributePresenterActionsKeyContainerManager(KeyStoreItem[] presentations)
        : base(presentations)
    {
    }

    public IPressenterAction GetValue<TPresentationAttribute>()
        where TPresentationAttribute : IPresentationAttribute

    {
        return GetValue(typeof(TPresentationAttribute));
    }

    public IPressenterAction GetValue(Type pressenterActionType)
    {
        if (TryGetValue(pressenterActionType, out var pressenterAction))
        {
            return pressenterAction;
        }
        else
        {
            throw new AppException($"Unregistered {pressenterActionType.FullName} type of presenter action.");
        }
    }

    public bool TryGetValue(Type pressenterActionType, [MaybeNullWhen(false)] out IPressenterAction pressenterAction)
    {
        return base.TryGetValue(pressenterActionType.TypeHandle.Value, out pressenterAction);
    }
}
