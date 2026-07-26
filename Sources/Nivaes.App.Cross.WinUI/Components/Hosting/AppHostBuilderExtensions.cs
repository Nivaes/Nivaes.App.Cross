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
            builder.Services.AddSingleton<CrossWindowsViewDispatcher>();
            builder.Services.AddSingleton<ICrossViewDispatcher>(sp =>
                sp.GetRequiredService<CrossWindowsViewDispatcher>());
            builder.Services.AddSingleton<ICrossMainThreadDispatcher>(sp =>
                sp.GetRequiredService<CrossWindowsViewDispatcher>());

            builder.Services.AddSingleton<ICrossWindowsFrame>(sp => new CrossWindowsFrame(app.RootFrame!));
            builder.Services.AddSingleton<IWindowsViewPresenterManager, MultiWindowViewPresenterManager>();
            builder.Services.AddSingleton<IPressenterActionContext, PressenterActionContext>();

            builder.Services.AddSingleton<ICrashHandler, WinUICrashHandler>();

            // Plugins
            builder.Services.AddSingleton<ICrossNativeColor, CrossWinUIColor>();
            builder.Services.AddSingleton<ICrossNativeVisibility, CrossWinUIVisibility>();

            builder.Services.AddSingleton<ICrossSuspensionManager, CrossSuspensionManager>();
            builder.Services.AddSingleton<ICrossWindowsViewModelLoader, CrossWindowsViewsContainer>();

            builder.Services.AddSingleton<ILoadDataService, LoadDataService>();
            builder.Services.AddSingleton<IBusyService, BusyService>();
            builder.Services.AddSingleton<IMediaService, MediaService>();

            builder.Services.AddSingleton<IBusyService, BusyService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IDeviceService, DeviceService>();
            builder.Services.AddSingleton<IMediaService, MediaService>();
            builder.Services.AddSingleton<ILoadDataService, LoadDataService>();

            return builder;
        }
    }
}
