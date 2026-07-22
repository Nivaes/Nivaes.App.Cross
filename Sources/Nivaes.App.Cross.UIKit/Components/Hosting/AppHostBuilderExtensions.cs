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
            builder.Services.AddSingleton<MvxIosViewDispatcher>();
            builder.Services.AddSingleton<ICrossViewDispatcher>(sp => sp.GetRequiredService<MvxIosViewDispatcher>());
            builder.Services.AddSingleton<ICrossMainThreadDispatcher>(sp => sp.GetRequiredService<MvxIosViewDispatcher>());

            builder.Services.AddSingleton<MvxIosViewDispatcher, MvxIosViewDispatcher>();

            builder.Services.AddSingleton<IIosViewPresenterManager, IosViewPresenterManager>();
            builder.Services.AddSingleton<IPressenterActionContext>(sp => new PressenterActionContext(windows));

            builder.Services.AddSingleton<IMvxIosViewCreator, MvxIosViewsContainer>();

            builder.Services.AddSingleton<ICrashHandler, UIKitCrashHandler>();

            // Plugins
            builder.Services.AddSingleton<ICrossNativeColor, MvxIosColor>();
            builder.Services.AddSingleton<ICrossNativeVisibility, MvxIosVisibility>();


            return builder;
        }
    }
}
