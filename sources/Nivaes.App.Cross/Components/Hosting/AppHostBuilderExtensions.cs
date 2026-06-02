using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nivaes.App.Cross.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseCrossApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this CrossAppBuilder builder)
        where TApp : class, IApplication
        {
            builder.Services.TryAddSingleton<IApplication, TApp>();
            builder.SetupDefaults();
            
            return builder;
        }

        public static CrossAppBuilder UseCrossApp<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this CrossAppBuilder builder, Func<IServiceProvider, TApp> implementationFactory)
        where TApp : class, IApplication
        {
            builder.Services.TryAddSingleton<IApplication>(implementationFactory);
            builder.SetupDefaults();

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder)
        {

            // ToDo: Initialize default services for hosting, logging, configuration, etc. if needed.
            builder.Services.TryAddSingleton<ICrossNavigationService, CrossNavigationService>();
            builder.Services.TryAddSingleton<ICrossViewModelLoader, CrossViewModelLoader>();
            builder.Services.TryAddSingleton<ICrossViewModelLocator, CrossDefaultViewModelLocator>();


            //bootstrapper.AddSingleton<ILoggingService, LoggingService>();
            //bootstrapper.AddSingleton<ICrossNavigationService, CrossNavigationService>();

            //bootstrapper.AddSingleton<ICrossSettings, CrossSettings>();
            //bootstrapper.AddSingleton<ICrossStringToTypeParser, CrossStringToTypeParser>();
            //bootstrapper.AddSingleton<ICrossViewModelLoader, CrossViewModelLoader>();
            //bootstrapper.AddSingleton<ICrossResultViewModelManager, CrossResultViewModelManager>();
            //bootstrapper.AddSingleton<ICrossViewModelTypeFinder, CrossViewModelViewTypeFinder>();
            //bootstrapper.AddSingleton<ICrossViewModelByNameLookup, CrossViewModelByNameLookup>();
            ////bootstrapper.AddSingleton<ICrossViewModelByNameRegistry, CrossViewModelByNameLookup>();
            //bootstrapper.AddSingleton<ICrossTypeToTypeLookupBuilder, CrossViewModelViewLookupBuilder>();
            //bootstrapper.AddSingleton<ICrossCommandCollectionBuilder, CrossCommandCollectionBuilder>();
            //bootstrapper.AddSingleton<ICrossNavigationSerializer, CrossStringDictionaryNavigationSerializer>();
            //bootstrapper.AddSingleton<ICrossChildViewModelCache, CrossChildViewModelCache>();

            return builder;
        }

    }
}
