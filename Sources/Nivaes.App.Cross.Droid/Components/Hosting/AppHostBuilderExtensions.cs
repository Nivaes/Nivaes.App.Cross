using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Droid.Observability;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.Observability;

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
            builder.Services.TryAddSingleton<IAndroidViewPresenterManager, AndroidViewPresenterManager>();

            builder.Services.TryAddSingleton<IMvxAndroidCurrentTopActivity, MvxCurrentTopActivity>();
            builder.Services.TryAddSingleton<IMvxAndroidActivityLifetimeListener, MvxAndroidLifetimeMonitor>();

            // ToDo: Unificar interfaces.
            builder.Services.TryAddSingleton<AndroidViewsContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AndroidViewsContainer>>();
                var navigationSerializer = sp.GetRequiredService<ICrossNavigationSerializer>();
                var childViewModelCache = sp.GetRequiredService<ICrossChildViewModelCache>();

                return new AndroidViewsContainer(applicationContext, navigationSerializer, childViewModelCache, logger);
            });
            builder.Services.TryAddSingleton<ICrossViewsContainer>(sp =>
                sp.GetRequiredService<AndroidViewsContainer>());
            builder.Services.TryAddSingleton<IMvxAndroidViewModelRequestTranslator>(sp =>
                sp.GetRequiredService<AndroidViewsContainer>());
            builder.Services.TryAddSingleton<IMvxAndroidViewModelLoader>(sp =>
                sp.GetRequiredService<AndroidViewsContainer>());

            builder.Services.TryAddSingleton<ICrashHandler, AndroidCrashHandler>();
            builder.Services.TryAddSingleton<ICrossBindingContextStack<IMvxAndroidBindingContext>, MvxAndroidBindingContextStack>();
            builder.Services.TryAddSingleton<IMvxSingleViewModelCache, MvxSingleViewModelCache>();
            builder.Services.TryAddSingleton<IMvxIntentResultSink, MvxIntentResultSink>();
            builder.Services.TryAddSingleton<IMvxSavedStateConverter, MvxSavedStateConverter>();
            builder.Services.TryAddSingleton<ICrossBinder, CrossFromTextBinder>();
            builder.Services.TryAddSingleton<IMvxAndroidViewFactory, MvxAndroidViewFactory>();
            builder.Services.TryAddSingleton<IMvxAndroidBindingResource, MvxAndroidBindingResource>();

            builder.Services.TryAddSingleton<IMvxViewTypeResolver, CrossViewTypeResolver>();

            builder.Services.TryAddSingleton<ICrossSourceBindingFactoryExtension, CrossPropertySourceBindingFactoryExtension>();

            builder.Services.TryAddSingleton<ICrossMainThreadAsyncDispatcher, MvxAndroidViewDispatcher>();
            builder.Services.TryAddSingleton<IMvxMultipleViewModelCache, MvxMultipleViewModelCache>();

            // Plugins
            builder.Services.TryAddSingleton<ICrossNativeColor, MvxAndroidColor>();
            builder.Services.TryAddSingleton<ICrossNativeVisibility, CrossDroidVisibility>();

            return builder;
        }
    }
}
