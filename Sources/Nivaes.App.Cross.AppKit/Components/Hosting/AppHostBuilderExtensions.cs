using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib.Observability;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross.AppKitLib
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseAppKitApp(this CrossAppBuilder builder, INSApplicationDelegate applicationDelegation)
        {
            builder.SetupDefaults(applicationDelegation);

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, INSApplicationDelegate applicationDelegation)
        {
            builder.Services.TryAddSingleton<MvxMacViewDispatcher>();
            builder.Services.TryAddSingleton<ICrossViewDispatcher>(sp =>
                    sp.GetRequiredService<MvxMacViewDispatcher>());
            builder.Services.TryAddSingleton<ICrossMainThreadAsyncDispatcher>(sp =>
                    sp.GetRequiredService<MvxMacViewDispatcher>());

            builder.Services.TryAddSingleton<IMvxMacViewPresenter>(sp =>
            {
                var viewsContainer = sp.GetRequiredService<ICrossViewsContainer>();
                var viewCreator = sp.GetRequiredService<IMvxMacViewCreator>();
                var logger = sp.GetRequiredService<ILogger<MvxMacViewPresenter>>();

                return new MvxMacViewPresenter(applicationDelegation, viewsContainer, logger);
            });

            builder.Services.TryAddSingleton<ICrossViewsContainer, MvxMacViewsContainer>();

            builder.Services.TryAddSingleton<ICrashHandler, AppKitCrashHandler>();
            builder.Services.TryAddSingleton<IMvxMacViewCreator, MvxMacViewsContainer>();
            
            // Plugins
            builder.Services.TryAddSingleton<ICrossNativeColor, CrossMacColor>();
            builder.Services.TryAddSingleton<ICrossNativeVisibility, CrossMacVisibility>();


            return builder;
        }
    }
}
