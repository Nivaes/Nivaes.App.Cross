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

        public MvxFluentBindingDescription<TTarget> CreateBinding(
    )
        {
            return new MvxFluentBindingDescription<TTarget>(target, target);
        }
    }

    public static MvxFluentBindingDescription<TTarget> CreateBinding<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
            this ICrossBindingContextOwner contextOwner, TTarget target)
                where TTarget : class
    {
        return new MvxFluentBindingDescription<TTarget>(contextOwner, target);
    }
}
