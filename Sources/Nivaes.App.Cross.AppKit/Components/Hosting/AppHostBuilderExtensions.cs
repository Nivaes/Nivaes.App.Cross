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
            
                
            builder.Services.AddSingleton<ICrossViewDispatcher, MvxMacViewDispatcher>();
            builder.Services.AddSingleton<ICrossMainThreadDispatcher, MvxMacViewDispatcher>();

            builder.Services.AddSingleton<IMacViewPresenterManager>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MacViewPresenterManager>>();

                return new MacViewPresenterManager(applicationDelegation, logger);
            });
            builder.Services.AddSingleton<IPressenterActionContext, PressenterActionContext>();

            builder.Services.AddSingleton<ICrashHandler, AppKitCrashHandler>();
            builder.Services.AddSingleton<IMvxMacViewCreator, MvxMacViewsContainer>();

            // Plugins
            builder.Services.AddSingleton<ICrossNativeColor, CrossMacColor>();
            builder.Services.AddSingleton<ICrossNativeVisibility, CrossMacVisibility>();


            return builder;
        }
    }
}
