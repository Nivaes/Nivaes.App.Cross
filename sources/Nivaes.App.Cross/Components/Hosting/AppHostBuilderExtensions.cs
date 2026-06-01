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

            return builder;
        }

    }
}
