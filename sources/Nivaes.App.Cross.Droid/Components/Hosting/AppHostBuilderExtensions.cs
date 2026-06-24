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


            builder.Services.TryAddSingleton<AndroidViewsContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<AndroidViewsContainer>>();
                var navigationSerializer = sp.GetRequiredService<ICrossNavigationSerializer>();
                var childViewModelCache = sp.GetRequiredService<ICrossChildViewModelCache>();

                return new AndroidViewsContainer(applicationContext, navigationSerializer, childViewModelCache, logger);
            });
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
            builder.Services.TryAddSingleton<IMvxLayoutInflaterHolderFactoryFactory, MvxLayoutInflaterFactoryFactory>();
            builder.Services.TryAddSingleton<IMvxAndroidViewBinderFactory, MvxAndroidViewBinderFactory>();
            builder.Services.TryAddSingleton<IMvxAndroidBindingResource, MvxAndroidBindingResource>();

            //builder.Services.TryAddSingleton<IMvxTypeCache, MvxTypeCache<View>>();
            //builder.Services.TryAddSingleton<IMvxAxmlNameViewTypeResolver, MvxAxmlNameViewTypeResolver>();
            //builder.Services.TryAddSingleton<IMvxNamespaceListViewTypeResolver, MvxNamespaceListViewTypeResolver>();
            //builder.Services.TryAddSingleton<MvxReflectionViewTypeResolver, MvxJustNameViewTypeResolver>();


            //ToDo: Refactorizar esto. Hay clases con el mismo interface que están anidadas.

            //builder.Services.TryAddSingleton<IMvxViewTypeResolver>(sp =>
            //     {
            //         var fullNameViewTypeResolver = (MvxAxmlNameViewTypeResolver)sp.GetRequiredService<IMvxAxmlNameViewTypeResolver>();
            //         var listViewTypeResolver = (MvxNamespaceListViewTypeResolver)sp.GetRequiredService<IMvxNamespaceListViewTypeResolver>();
            //         var justNameTypeResolver = sp.GetRequiredService<MvxReflectionViewTypeResolver>();

            //         var composite = new MvxCompositeViewTypeResolver(fullNameViewTypeResolver, listViewTypeResolver, justNameTypeResolver);
            //         return composite;
            //     });

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
