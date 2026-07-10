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

        return stack?.Current!;
    }
}
