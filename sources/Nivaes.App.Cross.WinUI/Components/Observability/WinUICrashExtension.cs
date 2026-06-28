using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Nivaes.App.Cross.Observability;
using Nivaes.App.Cross.WinUI.Observability;

namespace Nivaes.App.Cross.WinUI
{
    internal static class WinUICrashExtension
    {
        internal static void RegisterWinUICrash(this IServiceProvider service, Application app)
        {
            var crashHandler = (WinUICrashHandler)service.GetRequiredService<ICrashHandler>();

            crashHandler.RegisterApplication(app);
        }
    }
}
