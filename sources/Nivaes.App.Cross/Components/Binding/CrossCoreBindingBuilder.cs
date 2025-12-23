namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using MvvmCross.Binding.Parse.Binding.Tibet;
    using MvvmCross.IoC;

    public class CrossCoreBindingBuilder
    {
        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public virtual void DoRegistration(IMvxIoCProvider iocProvider)
        {
            CreateSingleton();
            RegisterCore(iocProvider);
            RegisterValueConverterRegistryFiller(iocProvider);
            RegisterValueConverterProvider(iocProvider);
            RegisterValueCombinerRegistryFiller(iocProvider);
            RegisterValueCombinerProvider(iocProvider);
            RegisterAutoValueConverters(iocProvider);
            RegisterBindingParser(iocProvider);
            RegisterLanguageBindingParser(iocProvider);
            RegisterBindingDescriptionParser(iocProvider);
            RegisterExpressionParser(iocProvider);
            RegisterSourcePropertyPathParser(iocProvider);
            RegisterPlatformSpecificComponents(iocProvider);
            RegisterBindingNameRegistry(iocProvider);
        }

        protected virtual void RegisterAutoValueConverters(IMvxIoCProvider iocProvider)
        {
            var autoValueConverters = CreateAutoValueConverters();
            iocProvider.RegisterSingleton<ICrossAutoValueConverters>(autoValueConverters);
            FillAutoValueConverters(autoValueConverters);
        }

        protected virtual void FillAutoValueConverters(ICrossAutoValueConverters autoValueConverters)
        {
            // nothing to do in base class
        }

        protected virtual ICrossAutoValueConverters CreateAutoValueConverters()
        {
            return new CrossAutoValueConverters();
        }

        protected virtual void CreateSingleton()
        {
            CrossBindingSingletonCache.Initialize();
        }

        protected virtual void RegisterValueConverterRegistryFiller(IMvxIoCProvider iocProvider)
        {
            var filler = CreateValueConverterRegistryFiller();
            iocProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<ICrossValueConverter>>(filler);
            iocProvider.RegisterSingleton<ICrossValueConverterRegistryFiller>(filler);
        }

        protected virtual ICrossValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new CrossValueConverterRegistryFiller();
        }

        protected virtual void RegisterValueCombinerRegistryFiller(IMvxIoCProvider iocProvider)
        {
            var filler = CreateValueCombinerRegistryFiller();
            iocProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<ICrossValueCombiner>>(filler);
            iocProvider.RegisterSingleton<ICrossValueCombinerRegistryFiller>(filler);
        }

        protected virtual ICrossValueCombinerRegistryFiller CreateValueCombinerRegistryFiller()
        {
            return new CrossValueCombinerRegistryFiller();
        }

        protected virtual void RegisterExpressionParser(IMvxIoCProvider iocProvider)
        {
            iocProvider.RegisterType<ICrossPropertyExpressionParser, CrossPropertyExpressionParser>();
        }

        protected virtual void RegisterCore(IMvxIoCProvider iocProvider)
        {
            iocProvider.RegisterSingleton<ICrossBinder>(new CrossFromTextBinder());
            iocProvider.RegisterType<ICrossBindingContext, MvxTaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider(IMvxIoCProvider iocProvider)
        {
            var registry = CreateValueConverterRegistry();
            iocProvider.RegisterSingleton<IMvxNamedInstanceLookup<ICrossValueConverter>>(registry);
            iocProvider.RegisterSingleton<ICrossNamedInstanceRegistry<ICrossValueConverter>>(registry);
            iocProvider.RegisterSingleton<ICrossValueConverterLookup>(registry);
            iocProvider.RegisterSingleton<ICrossValueConverterRegistry>(registry);
            FillValueConverters(registry);
        }

        protected virtual CrossValueConverterRegistry CreateValueConverterRegistry()
        {
            return new CrossValueConverterRegistry();
        }

        protected virtual void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("CommandParameter", new CrossCommandParameterValueConverter());
            registry.AddOrOverwrite("Language", new CrossLanguageConverter());
        }

        protected virtual void RegisterValueCombinerProvider(IMvxIoCProvider iocProvider)
        {
            var registry = CreateValueCombinerRegistry();
            iocProvider.RegisterSingleton<IMvxNamedInstanceLookup<ICrossValueCombiner>>(registry);
            iocProvider.RegisterSingleton<ICrossNamedInstanceRegistry<ICrossValueCombiner>>(registry);
            iocProvider.RegisterSingleton<ICrossValueCombinerLookup>(registry);
            iocProvider.RegisterSingleton<ICrossValueCombinerRegistry>(registry);
            FillValueCombiners(registry);
        }

        protected virtual ICrossValueCombinerRegistry CreateValueCombinerRegistry()
        {
            return new CrossValueCombinerRegistry();
        }

        protected virtual void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            // note that assembly based registration is not used here for efficiency reasons
            // - see #327 - https://github.com/slodge/MvvmCross/issues/327
            registry.AddOrOverwrite("Add", new CrossAddValueCombiner());
            registry.AddOrOverwrite("Divide", new CrossDivideValueCombiner());
            registry.AddOrOverwrite("Format", new CrossFormatValueCombiner());
            registry.AddOrOverwrite("If", new CrossIfValueCombiner());
            registry.AddOrOverwrite("Modulus", new CrossModulusValueCombiner());
            registry.AddOrOverwrite("Multiply", new CrossMultiplyValueCombiner());
            registry.AddOrOverwrite("Single", new CrossSingleValueCombiner());
            registry.AddOrOverwrite("Subtract", new CrossSubtractValueCombiner());
            registry.AddOrOverwrite("EqualTo", new CrossEqualToValueCombiner());
            registry.AddOrOverwrite("NotEqualTo", new MvxNotEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThanOrEqualTo", new CrossGreaterThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThan", new CrossGreaterThanValueCombiner());
            registry.AddOrOverwrite("LessThanOrEqualTo", new CrossLessThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("LessThan", new CrossLessThanValueCombiner());
            registry.AddOrOverwrite("Not", new MvxNotValueCombiner());
            registry.AddOrOverwrite("And", new MvxAndValueCombiner());
            registry.AddOrOverwrite("Or", new MvxOrValueCombiner());
            registry.AddOrOverwrite("XOr", new MvxXorValueCombiner());
            registry.AddOrOverwrite("Inverted", new MvxInvertedValueCombiner());

            // Note: MvxValueConverterValueCombiner is not registered - it is unconventional
            //registry.AddOrOverwrite("ValueConverter", new MvxValueConverterValueCombiner());
        }

        protected virtual void RegisterBindingParser(IMvxIoCProvider iocProvider)
        {
            if (iocProvider.CanResolve<ICrossBindingParser>())
            {
                CrossBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            CrossBindingLog.Instance?.LogTrace("Registering Default Binding Parser");
            iocProvider.RegisterSingleton(CreateBindingParser());
        }

        protected virtual ICrossBindingParser CreateBindingParser()
        {
            return new CrossTibetBindingParser();
        }

        protected virtual void RegisterLanguageBindingParser(IMvxIoCProvider iocProvider)
        {
            if (iocProvider.CanResolve<ICrossLanguageBindingParser>())
            {
                CrossBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Language parser");
                return;
            }
            CrossBindingLog.Instance?.LogTrace("Registering Language Binding Parser");
            iocProvider.RegisterSingleton(CreateLanguageBindingParser());
        }

        protected virtual ICrossLanguageBindingParser CreateLanguageBindingParser()
        {
            return new CrossLanguageBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser(IMvxIoCProvider iocProvider)
        {
            var parser = CreateBindingDescriptionParser();
            iocProvider.RegisterSingleton(parser);
        }

        private static ICrossBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new CrossBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser(IMvxIoCProvider iocProvider)
        {
            var tokeniser = CreateSourcePropertyPathParser();
            iocProvider.RegisterSingleton<ICrossSourcePropertyPathParser>(tokeniser);
        }

        protected virtual ICrossSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new CrossSourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry(IMvxIoCProvider iocProvider)
        {
            var registry = new CrossBindingNameRegistry();
            iocProvider.RegisterSingleton<ICrossBindingNameLookup>(registry);
            iocProvider.RegisterSingleton<ICrossBindingNameRegistry>(registry);
            FillDefaultBindingNames(registry);
        }

        protected virtual void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterPlatformSpecificComponents(IMvxIoCProvider iocProvider)
        {
            // nothing to do here
        }
    }
}
