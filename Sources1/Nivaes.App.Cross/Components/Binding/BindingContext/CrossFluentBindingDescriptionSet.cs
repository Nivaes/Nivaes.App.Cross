using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public class CrossFluentBindingDescriptionSet<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TOwningTarget, TSource>
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
    public CrossFluentBindingDescription<TOwningTarget, TSource> Bind()
    {
        var toReturn = new CrossFluentBindingDescription<TOwningTarget, TSource>(
            _bindingContextOwner, _bindingContextOwner);
        _applicables.Add(toReturn);
        return toReturn;
    }

    public CrossFluentBindingDescription<TChildTarget, TSource> Bind<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(TChildTarget childTarget)
            where TChildTarget : class
    {
        var toReturn = new CrossFluentBindingDescription<TChildTarget, TSource>(_bindingContextOwner, childTarget);
        _applicables.Add(toReturn);
        return toReturn;
    }

    public CrossFluentBindingDescription<TChildTarget, TSource> Bind<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
            TChildTarget childTarget,
            string bindingDescription)
                where TChildTarget : class
    {
        var toReturn = Bind(childTarget);
        toReturn.FullyDescribed(bindingDescription);
        return toReturn;
    }

    public CrossFluentBindingDescription<TChildTarget, TSource> Bind<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TChildTarget>(
            TChildTarget childTarget, CrossBindingDescription bindingDescription)
                where TChildTarget : class
    {
        var toReturn = Bind(childTarget);
        toReturn.FullyDescribed(bindingDescription);
        return toReturn;
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public override void Apply()
    {
        foreach (var applicable in _applicables)
            applicable.Apply();
        base.Apply();
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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
                CrossBindingLogger.Instance?.LogWarning(
                    "Fluent binding description must implement {InterfaceName} in order to add {Description}",
                    nameof(ICrossBaseFluentBindingDescription),
                    nameof(ICrossBaseFluentBindingDescription.ClearBindingKey));
            }

            applicable.Apply();
        }

        base.Apply();
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Bindings inherently use reflection. This is by design and callers are warned through usage of binding methods.")]
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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
