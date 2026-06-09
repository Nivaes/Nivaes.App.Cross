using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Droid
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseDroidApp(this CrossAppBuilder builder, Context applicationContext)
        {
            builder.SetupDefaults(applicationContext);
            
            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder, Context applicationContext)
        {
            builder.Services.TryAddSingleton<ICrossViewDispatcher, MvxAndroidViewDispatcher>();
            builder.Services.TryAddSingleton<IAndroidViewPresenter, AndroidViewPresenter>();

            builder.Services.TryAddSingleton<ICrossViewsContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AndroidViewsContainer>>();
                var navigationSerializer = sp.GetRequiredService<ICrossNavigationSerializer>();
                var childViewModelCache = sp.GetRequiredService<ICrossChildViewModelCache>();

                return new AndroidViewsContainer(applicationContext, navigationSerializer, childViewModelCache, logger);
            });
            builder.Services.TryAddSingleton<IMvxAndroidCurrentTopActivity, MvxCurrentTopActivity>();
            builder.Services.TryAddSingleton<IMvxAndroidActivityLifetimeListener, MvxAndroidLifetimeMonitor>();
            builder.Services.TryAddSingleton<IMvxAndroidViewModelRequestTranslator>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AndroidViewsContainer>>();
                var navigationSerializer = sp.GetRequiredService<ICrossNavigationSerializer>();
                var childViewModelCache = sp.GetRequiredService<ICrossChildViewModelCache>();

                return new AndroidViewsContainer(applicationContext, navigationSerializer, childViewModelCache, logger);
            });

            builder.Services.TryAddSingleton<ICrashHandler, AndroidCrashHandler>();

            return builder;
        } 
    }
}
