using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class CrashHandlerExtension
    {
        public static void SetupCrash(this IServiceProvider serviceProvider)
        {
            var crashHandler = serviceProvider.GetRequiredService<ICrashHandler>();
            crashHandler.Register();
        }
    }
}
