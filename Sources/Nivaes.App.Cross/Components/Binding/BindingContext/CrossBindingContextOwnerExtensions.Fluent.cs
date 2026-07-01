using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross;

public static partial class CrossBindingContextOwnerExtensions
{
    public static CrossFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
            this TTarget target)
                where TTarget : class, ICrossBindingContextOwner
    {
        return new CrossFluentBindingDescriptionSet<TTarget, TSource>(target);
    }

    public static CrossFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
            this TTarget target, string clearBindingKey)
                where TTarget : class, ICrossBindingContextOwner
    {
        return new CrossFluentBindingDescriptionSet<TTarget, TSource>(target, clearBindingKey);
    }

    public static MvxFluentBindingDescription<TTarget> CreateBinding<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
        this TTarget target)
            where TTarget : class, ICrossBindingContextOwner
    {
        return new MvxFluentBindingDescription<TTarget>(target, target);
    }

    public static MvxFluentBindingDescription<TTarget> CreateBinding<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
            this ICrossBindingContextOwner contextOwner, TTarget target)
                where TTarget : class
    {
        return new MvxFluentBindingDescription<TTarget>(contextOwner, target);
    }
}
