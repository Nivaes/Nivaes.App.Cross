using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;
using Nivaes.App.Cross.UIKitLib.Observability;

namespace Nivaes.App.Cross.UIKitLib
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseUIKitApp(this CrossAppBuilder builder, UIWindow windows)
        {
            builder.SetupDefaults(windows);

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, UIWindow windows)
        {
            builder.Services.TryAddSingleton<MvxIosViewDispatcher>();
            builder.Services.TryAddSingleton<ICrossViewDispatcher>(sp =>
                    sp.GetRequiredService<MvxIosViewDispatcher>());
            builder.Services.TryAddSingleton<ICrossMainThreadAsyncDispatcher>(sp =>
                    sp.GetRequiredService<MvxIosViewDispatcher>());

            builder.Services.TryAddSingleton<IIosViewPresenterManager, IosViewPresenterManager>();
            builder.Services.AddSingleton<IPressenterActionContext>(sp => new PressenterActionContext(windows));
            
            builder.Services.TryAddSingleton<ICrossViewsContainer, MvxIosViewsContainer>();
            builder.Services.TryAddSingleton<IMvxIosViewCreator, MvxIosViewsContainer>();

            builder.Services.TryAddSingleton<ICrashHandler, UIKitCrashHandler>();

            // Plugins
            builder.Services.TryAddSingleton<ICrossNativeColor, MvxIosColor>();
            builder.Services.TryAddSingleton<ICrossNativeVisibility, MvxIosVisibility>();


            return builder;
        }
    }
}
