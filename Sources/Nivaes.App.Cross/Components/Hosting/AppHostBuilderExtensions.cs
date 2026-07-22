using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Nivaes.App.Cross.Hosting
{
    public static class AppHostBuilderExtensions
    {
        public static CrossAppBuilder UseCrossApp<TApp>(this CrossAppBuilder builder)
            where TApp : class, ICrossApplication
        {
            builder.Services.TryAddSingleton<ICrossApplication, TApp>();
            builder.SetupDefaults();

            return builder;
        }

        public static CrossAppBuilder UseCrossApp<TApp>(this CrossAppBuilder builder, Func<IServiceProvider, TApp> implementationFactory)
        where TApp : class, ICrossApplication
        {
            builder.Services.TryAddSingleton<ICrossApplication>(implementationFactory);
            builder.SetupDefaults();

            return builder;
        }

        static CrossAppBuilder SetupDefaults(this CrossAppBuilder builder)
        {

            // ToDo: Initialize default services for hosting, logging, configuration, etc. if needed.
            builder.Services.TryAddSingleton<CrossNavigationService, CrossNavigationService>();
            builder.Services.TryAddSingleton<CrossViewModelLoader>();
            builder.Services.TryAddSingleton<CrossViewModelLocator>();

            builder.Services.TryAddSingleton<ICrossResultViewModelManager, CrossResultViewModelManager>();

            builder.Services.TryAddTransient<ICrossBindingContext, CrossTaskBasedBindingContext>();

            builder.SetupBinding();

            return builder;
        }

        static CrossAppBuilder SetupBinding(this CrossAppBuilder builder)
        {
            // ToDo: Refactorizar esto (posiblemente merezca la pena crear un almacen separado para binding)
            builder.Services.AddSingleton<ICrossBindingDescriptionParser, CrossBindingDescriptionParser>();
            builder.Services.AddSingleton<ICrossBindingParser, CrossTibetBindingParser>();
            builder.Services.AddSingleton<ICrossSourceBindingFactory, CrossSourceBindingFactory>();

            var targetBindingFactoryRegistry = new CrossTargetBindingFactoryRegistry();
            builder.Services.AddSingleton<ICrossTargetBindingFactoryRegistry>(targetBindingFactoryRegistry);
            builder.Services.AddSingleton<ICrossTargetBindingFactory>(targetBindingFactoryRegistry);

            builder.Services.TryAddSingleton<ICrossSourcePropertyPathParser, CrossSourcePropertyPathParser>();

            // ToDo: Refactorizar esto (posiblemente merezca la pena crear un almacen separado para binding)
            var sourceStepFactory = new CrossSourceStepFactory();
            sourceStepFactory.AddOrOverwrite(typeof(CrossCombinerSourceStepDescription), new CrossCombinerSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(CrossPathSourceStepDescription), new CrossPathSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(CrossLiteralSourceStepDescription), new CrossLiteralSourceStepFactory());
            builder.Services.AddSingleton<ICrossSourceStepFactoryRegistry>(sourceStepFactory);
            builder.Services.AddSingleton<ICrossSourceStepFactory>(sourceStepFactory);

            builder.Services.AddTransient<ICrossPropertyExpressionParser, CrossPropertyExpressionParser>();

            var bindingNameRegistry = new CrossBindingNameRegistry();
            builder.Services.AddSingleton<ICrossBindingNameLookup>(bindingNameRegistry);
            builder.Services.AddSingleton<ICrossBindingNameRegistry>(bindingNameRegistry);

            builder.Services.AddSingleton<ICrossBinder, CrossFromTextBinder>();
            builder.Services.AddSingleton<ICrossSourceBindingFactoryExtension, CrossPropertySourceBindingFactoryExtension>();

            return builder;
        }
    }
}
