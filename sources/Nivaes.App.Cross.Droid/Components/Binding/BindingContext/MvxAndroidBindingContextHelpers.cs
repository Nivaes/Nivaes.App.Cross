using Microsoft.Extensions.DependencyInjection;

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
        var stack = IPlatformApplication.Current!.Services.GetRequiredService<ICrossBindingContextStack<T>>();
        //if (Mvx.IoCProvider?.TryResolve<ICrossBindingContextStack<T>>(out var stack) == true)

        return stack?.Current!;
    }
}
