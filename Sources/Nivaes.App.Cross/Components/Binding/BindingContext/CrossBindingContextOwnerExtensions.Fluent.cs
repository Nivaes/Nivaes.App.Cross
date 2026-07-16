using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public static partial class CrossBindingContextOwnerExtensions
{
    extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(TTarget target)
        where TTarget : class, ICrossBindingContextOwner
    {
        public CrossFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TSource>()
        {
            return new CrossFluentBindingDescriptionSet<TTarget, TSource>(target);
        }

        public CrossFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<TSource>(string clearBindingKey)
        {
            return new CrossFluentBindingDescriptionSet<TTarget, TSource>(target, clearBindingKey);
        }

        public FluentBindingDescription<TTarget> CreateBinding(
    )
        {
            return new FluentBindingDescription<TTarget>(target, target);
        }
    }

    public static FluentBindingDescription<TTarget> CreateBinding<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
            this ICrossBindingContextOwner contextOwner, TTarget target)
                where TTarget : class
    {
        return new FluentBindingDescription<TTarget>(contextOwner, target);
    }
}
