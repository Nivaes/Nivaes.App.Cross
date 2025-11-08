namespace Nivaes.App.Cross.Droid
{
    public static class CrossAndroidBindingContextHelpers
    {
        public static ICrossAndroidBindingContext Current()
        {
            return Current<ICrossAndroidBindingContext>();
        }

        public static T Current<T>()
            where T : class, ICrossBindingContext
        {
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve<IMvxBindingContextStack<T>>(out var stack) == true)
            //    return stack?.Current;

            //return null;
        }
    }
}
