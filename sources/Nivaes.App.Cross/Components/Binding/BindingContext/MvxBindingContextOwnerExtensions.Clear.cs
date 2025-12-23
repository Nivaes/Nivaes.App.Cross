namespace Nivaes.App.Cross
{
    public static partial class MvxBindingContextOwnerExtensions
    {
        public static void ClearBindings(this IMvxBindingContextOwner owner, object target)
        {
            owner.BindingContext.ClearBindings(target);
        }

        public static void ClearAllBindings(this IMvxBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}
