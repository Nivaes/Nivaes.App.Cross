namespace Nivaes.App.Cross
{
    public static partial class CrossBindingContextOwnerExtensions
    {
        public static void ClearBindings(this ICrossBindingContextOwner owner, object target)
        {
            owner.BindingContext.ClearBindings(target);
        }

        public static void ClearAllBindings(this ICrossBindingContextOwner owner)
        {
            owner.BindingContext.ClearAllBindings();
        }
    }
}
