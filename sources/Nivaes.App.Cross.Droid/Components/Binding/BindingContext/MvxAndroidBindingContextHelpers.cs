namespace Nivaes.App.Cross.Droid
{
    using MvvmCross;
    using MvvmCross.Binding.BindingContext;

    public static class MvxAndroidBindingContextHelpers
    {
        public static IMvxAndroidBindingContext Current()
        {
            return Current<IMvxAndroidBindingContext>();
        }

        public static T Current<T>()
            where T : class, IMvxBindingContext
        {
            if (Mvx.IoCProvider?.TryResolve<IMvxBindingContextStack<T>>(out var stack) == true)
                return stack?.Current;

            return null;
        }
    }
}
