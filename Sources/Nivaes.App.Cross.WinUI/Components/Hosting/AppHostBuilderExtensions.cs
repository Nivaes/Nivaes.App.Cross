using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;
using Nivaes.App.Cross.WinUI.Observability;

namespace Nivaes.App.Cross.WinUI
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseWinUIApp(this CrossAppBuilder builder, CrossWinUIApplication app)
        {
            builder.SetupDefaults(app);

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, CrossWinUIApplication app)
        {
            // ToDo: Unificar interfaces.
            builder.Services.TryAddSingleton<CrossWindowsViewDispatcher>();
            builder.Services.TryAddSingleton<ICrossViewDispatcher>(sp =>
                sp.GetRequiredService<CrossWindowsViewDispatcher>());
            builder.Services.TryAddSingleton<ICrossMainThreadAsyncDispatcher>(sp =>
                sp.GetRequiredService<CrossWindowsViewDispatcher>());

            builder.Services.TryAddSingleton<ICrossWindowsFrame>(sp => new CrossWindowsFrame(app.RootFrame!));
            builder.Services.TryAddSingleton<IMvxWindowsViewPresenter, MvxMultiWindowViewPresenter>();
            builder.Services.TryAddSingleton<ICrossViewsContainer, CrossWindowsViewsContainer>();

            builder.Services.TryAddSingleton<ICrossWindowsViewModelRequestTranslator, CrossWindowsViewsContainer>();

            builder.Services.TryAddSingleton<ICrashHandler, WinUICrashHandler>();

            // Plugins
            builder.Services.TryAddSingleton<ICrossNativeColor, CrossWinUIColor>();
            builder.Services.TryAddSingleton<ICrossNativeVisibility, CrossWinUIVisibility>();

            builder.Services.TryAddSingleton<ICrossSuspensionManager, CrossSuspensionManager>();
            builder.Services.TryAddSingleton<ICrossWindowsViewModelLoader, CrossWindowsViewsContainer>();

            return builder;
        }
    }
}
