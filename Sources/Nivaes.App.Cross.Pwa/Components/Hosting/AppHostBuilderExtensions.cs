using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;
using Nivaes.App.Cross.Web.Observability;

namespace Nivaes.App.Cross.Web
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseWinUIApp(this CrossAppBuilder builder, CrossApplication app)
        {
            builder.SetupDefaults(app);

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, CrossApplication app)
        {
            //builder.Services.TryAddSingleton<ICrossViewDispatcher, CrossWindowsViewDispatcher>();

            //builder.Services.TryAddSingleton<ICrossWindowsFrame>(sp => new CrossWindowsFrame(app.RootFrame!));
            //builder.Services.TryAddSingleton<IMvxWindowsViewPresenter, MvxMultiWindowViewPresenter>();
            //builder.Services.TryAddSingleton<ICrossViewsContainer, CrossWindowsViewsContainer>();

            //builder.Services.TryAddSingleton<ICrossWindowsViewModelRequestTranslator, CrossWindowsViewsContainer>();

            builder.Services.TryAddSingleton<ICrashHandler, WebCrashHandler>();

            return builder;
        }
    }
}
