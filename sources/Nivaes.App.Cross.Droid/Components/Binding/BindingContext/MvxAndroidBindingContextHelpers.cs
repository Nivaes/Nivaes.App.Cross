using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

public static class MvxAndroidBindingContextHelpers
{
    public static IMvxAndroidBindingContext Current()
    {
        return Current<IMvxAndroidBindingContext>();
    }

    public static T Current<T>()
        where T : class, ICrossBindingContext
    {
        if (Mvx.IoCProvider?.TryResolve<ICrossBindingContextStack<T>>(out var stack) == true)
            return stack?.Current;

        return null;
    }
}
