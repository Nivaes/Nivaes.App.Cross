namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.BindingContext;

    public static partial class MvxBindingContextOwnerExtensions
    {
        public static MvxFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
                this TTarget target)
                    where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescriptionSet<TTarget, TSource>(target);
        }

        public static MvxFluentBindingDescriptionSet<TTarget, TSource> CreateBindingSet<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget, TSource>(
                this TTarget target, string clearBindingKey)
                    where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescriptionSet<TTarget, TSource>(target, clearBindingKey);
        }

        public static MvxFluentBindingDescription<TTarget> CreateBinding<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
            this TTarget target)
                where TTarget : class, IMvxBindingContextOwner
        {
            return new MvxFluentBindingDescription<TTarget>(target, target);
        }

        public static MvxFluentBindingDescription<TTarget> CreateBinding<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] TTarget>(
                this IMvxBindingContextOwner contextOwner, TTarget target)
                    where TTarget : class
        {
            return new MvxFluentBindingDescription<TTarget>(contextOwner, target);
        }
    }
}
