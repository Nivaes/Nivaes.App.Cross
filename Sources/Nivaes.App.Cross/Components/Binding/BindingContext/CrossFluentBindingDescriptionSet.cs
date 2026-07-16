using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossFluentBindingDescriptionSet<TOwningTarget, TSource>
        : CrossApplicable, IDisposable
            where TOwningTarget : class, ICrossBindingContextOwner
{
    private readonly List<ICrossApplicable> _applicables = [];
    private readonly TOwningTarget? _bindingContextOwner;
    private readonly string? _clearBindingKey;

    public CrossFluentBindingDescriptionSet(TOwningTarget bindingContextOwner)
    {
        _bindingContextOwner = bindingContextOwner;
    }
    public CrossFluentBindingDescriptionSet(TOwningTarget bindingContextOwner, string clearBindingKey) : this(bindingContextOwner)
    {
        _clearBindingKey = clearBindingKey;
    }
    public FluentBindingDescription<TOwningTarget, TSource> Bind()
    {
        var toReturn = new FluentBindingDescription<TOwningTarget, TSource>(
            _bindingContextOwner, _bindingContextOwner);
        _applicables.Add(toReturn);
        return toReturn;
    }

    public FluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(TChildTarget childTarget)
            where TChildTarget : class
    {
        var toReturn = new FluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
        _applicables.Add(toReturn);
        return toReturn;
    }

    public FluentBindingDescription<TChildTarget, TSource> Bind<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
            TChildTarget childTarget,
            string bindingDescription)
                where TChildTarget : class
    {
        var toReturn = Bind(childTarget);
        toReturn.FullyDescribed(bindingDescription);
        return toReturn;
    }

    public FluentBindingDescription<TChildTarget, TSource> Bind<TChildTarget>(
            TChildTarget childTarget, CrossBindingDescription bindingDescription)
                where TChildTarget : class
    {
        var toReturn = Bind(childTarget);
        toReturn.FullyDescribed(bindingDescription);
        return toReturn;
    }

    public override void Apply()
    {
        foreach (var applicable in _applicables)
            applicable.Apply();
        base.Apply();
    }

    public void ApplyWithClearBindingKey(object clearBindingKey)
    {
        foreach (var applicable in _applicables)
        {
            if (applicable is ICrossBaseFluentBindingDescription fluentBindingDescription)
            {
                fluentBindingDescription.ClearBindingKey = clearBindingKey;
            }
            else
            {
                CrossBindingLogger.GetLogger<CrossFluentBindingDescriptionSet<TOwningTarget, TSource>>().LogWarning(
                    "Fluent binding description must implement {InterfaceName} in order to add {Description}",
                    nameof(ICrossBaseFluentBindingDescription),
                    nameof(ICrossBaseFluentBindingDescription.ClearBindingKey));
            }

            applicable.Apply();
        }

        base.Apply();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (string.IsNullOrEmpty(_clearBindingKey))
                Apply();
            else
                ApplyWithClearBindingKey(_clearBindingKey);
        }
    }
}
