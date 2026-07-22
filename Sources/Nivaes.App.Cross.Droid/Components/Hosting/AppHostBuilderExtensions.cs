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
            builder.Services.AddSingleton<ICrossViewDispatcher, MvxAndroidViewDispatcher>();
            builder.Services.AddSingleton<ICrossMainThreadDispatcher, MvxAndroidViewDispatcher>();

            builder.Services.AddSingleton<IAndroidViewPresenterManager, AndroidViewPresenterManager>();
            builder.Services.AddSingleton<IPressenterActionContext, PressenterActionContext>();

            builder.Services.AddSingleton<IMvxAndroidCurrentTopActivity, MvxCurrentTopActivity>();
            builder.Services.AddSingleton<IMvxAndroidActivityLifetimeListener, MvxAndroidLifetimeMonitor>();

            // ToDo: Unificar interfaces.
            builder.Services.AddSingleton<AndroidViewsContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AndroidViewsContainer>>();

                return new AndroidViewsContainer(applicationContext, logger);
            });
            builder.Services.AddSingleton<IMvxAndroidViewModelRequestTranslator>(sp =>
                sp.GetRequiredService<AndroidViewsContainer>());
            builder.Services.AddSingleton<IMvxAndroidViewModelLoader>(sp =>
                sp.GetRequiredService<AndroidViewsContainer>());

            builder.Services.AddSingleton<ICrashHandler, AndroidCrashHandler>();
            builder.Services.AddSingleton<ICrossBindingContextStack<IMvxAndroidBindingContext>, MvxAndroidBindingContextStack>();
            builder.Services.AddSingleton<IMvxSingleViewModelCache, MvxSingleViewModelCache>();
            builder.Services.AddSingleton<IMvxIntentResultSink, MvxIntentResultSink>();
            builder.Services.AddSingleton<IMvxSavedStateConverter, MvxSavedStateConverter>();
            builder.Services.AddSingleton<ICrossBinder, CrossFromTextBinder>();
            builder.Services.AddSingleton<IMvxAndroidViewFactory, MvxAndroidViewFactory>();
            builder.Services.AddSingleton<IMvxAndroidBindingResource, MvxAndroidBindingResource>();

            builder.Services.AddSingleton<IMvxViewTypeResolver, CrossViewTypeResolver>();

            builder.Services.AddSingleton<ICrossSourceBindingFactoryExtension, CrossPropertySourceBindingFactoryExtension>();
            builder.Services.AddSingleton<IMvxMultipleViewModelCache, MvxMultipleViewModelCache>();

            // Plugins
            builder.Services.AddSingleton<ICrossNativeColor, MvxAndroidColor>();
            builder.Services.AddSingleton<ICrossNativeVisibility, CrossDroidVisibility>();

            builder.Services.AddSingleton<IBusyService, BusyService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IDeviceService, DeviceService>();
            builder.Services.AddSingleton<IMediaService, MediaService>();
            builder.Services.AddSingleton<ILoadDataService, LoadDataService>();

            return builder;
        }
    }
}
