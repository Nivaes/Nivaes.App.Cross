using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Win32;

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

            builder.Services.TryAddSingleton<ICrossResultViewModelManager, CrossResultViewModelManager>();

            builder.Services.TryAddSingleton<ICrossNavigationSerializer, CrossStringDictionaryNavigationSerializer>();
            builder.Services.TryAddSingleton<ICrossBindingContext, CrossTaskBasedBindingContext>();

            builder.SetupBinding();

            return builder;
        }

        static CrossAppBuilder SetupBinding(this CrossAppBuilder builder)
        {
            // ToDo: Refactorizar esto (posiblemente merezca la pena crear un almacen separado para binding)
            builder.Services.TryAddSingleton<ICrossChildViewModelCache, CrossChildViewModelCache>();
            builder.Services.TryAddSingleton<ICrossBindingDescriptionParser, CrossBindingDescriptionParser>();
            builder.Services.TryAddSingleton<ICrossBindingParser, CrossTibetBindingParser>();
            builder.Services.TryAddSingleton<ICrossSourceBindingFactory, CrossSourceBindingFactory>();

            var targetBindingFactoryRegistry = new CrossTargetBindingFactoryRegistry();
            builder.Services.TryAddSingleton<ICrossTargetBindingFactoryRegistry>(targetBindingFactoryRegistry);
            builder.Services.TryAddSingleton<ICrossTargetBindingFactory>(targetBindingFactoryRegistry);

            builder.Services.TryAddSingleton<ICrossSourcePropertyPathParser, CrossSourcePropertyPathParser>();

            // ToDo: Refactorizar esto (posiblemente merezca la pena crear un almacen separado para binding)
            var sourceStepFactory = new CrossSourceStepFactory();
            sourceStepFactory.AddOrOverwrite(typeof(CrossCombinerSourceStepDescription), new CrossCombinerSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(CrossPathSourceStepDescription), new CrossPathSourceStepFactory());
            sourceStepFactory.AddOrOverwrite(typeof(CrossLiteralSourceStepDescription), new CrossLiteralSourceStepFactory());
            builder.Services.TryAddSingleton<ICrossSourceStepFactoryRegistry>(sourceStepFactory);
            builder.Services.TryAddSingleton<ICrossSourceStepFactory>(sourceStepFactory);

            builder.Services.TryAddSingleton<ICrossPropertyExpressionParser, CrossPropertyExpressionParser>();

            var bindingNameRegistry = new CrossBindingNameRegistry();
            builder.Services.TryAddSingleton<ICrossBindingNameLookup>(bindingNameRegistry);
            builder.Services.TryAddSingleton<ICrossBindingNameRegistry>(bindingNameRegistry);

            

            var valueConverterRegistry = new CrossValueConverterRegistry();
            builder.Services.TryAddSingleton<ICrossValueConverterLookup>(valueConverterRegistry);
            builder.Services.TryAddSingleton<ICrossValueConverterRegistry>(valueConverterRegistry);
            builder.Services.TryAddSingleton<ICrossValueCombinerLookup, CrossValueCombinerRegistry>();
            builder.Services.TryAddSingleton<ICrossAutoValueConverters, CrossAutoValueConverters>();

            var valueCombinerRegistry = new CrossValueCombinerRegistry();
            builder.Services.TryAddSingleton<IMvxNamedInstanceLookup<ICrossValueCombiner>>(valueCombinerRegistry);
            builder.Services.TryAddSingleton<ICrossNamedInstanceRegistry<ICrossValueCombiner>>(valueCombinerRegistry);
            builder.Services.TryAddSingleton<ICrossValueCombinerLookup>(valueCombinerRegistry);
            builder.Services.TryAddSingleton<ICrossValueCombinerRegistry>(valueCombinerRegistry);

            return builder;
        }
    }
}
