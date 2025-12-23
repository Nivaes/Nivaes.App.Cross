namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Combiners;
    using MvvmCross.Binding.ExpressionParse;
    using MvvmCross.Binding.Parse.Binding.Lang;
    using MvvmCross.Binding.Parse.Binding.Tibet;
    using MvvmCross.Binding.Parse.PropertyPath;
    using MvvmCross.IoC;

    public class MvxCoreBindingBuilder
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
            MvxBindingSingletonCache.Initialize();
        }

        protected virtual void RegisterValueConverterRegistryFiller(IMvxIoCProvider iocProvider)
        {
            var filler = CreateValueConverterRegistryFiller();
            iocProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueConverter>>(filler);
            iocProvider.RegisterSingleton<ICrossValueConverterRegistryFiller>(filler);
        }

        protected virtual ICrossValueConverterRegistryFiller CreateValueConverterRegistryFiller()
        {
            return new CrossValueConverterRegistryFiller();
        }

        protected virtual void RegisterValueCombinerRegistryFiller(IMvxIoCProvider iocProvider)
        {
            var filler = CreateValueCombinerRegistryFiller();
            iocProvider.RegisterSingleton<IMvxNamedInstanceRegistryFiller<IMvxValueCombiner>>(filler);
            iocProvider.RegisterSingleton<IMvxValueCombinerRegistryFiller>(filler);
        }

        protected virtual IMvxValueCombinerRegistryFiller CreateValueCombinerRegistryFiller()
        {
            return new MvxValueCombinerRegistryFiller();
        }

        protected virtual void RegisterExpressionParser(IMvxIoCProvider iocProvider)
        {
            iocProvider.RegisterType<IMvxPropertyExpressionParser, MvxPropertyExpressionParser>();
        }

        protected virtual void RegisterCore(IMvxIoCProvider iocProvider)
        {
            iocProvider.RegisterSingleton<ICrossBinder>(new CrossFromTextBinder());
            iocProvider.RegisterType<ICrossBindingContext, MvxTaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider(IMvxIoCProvider iocProvider)
        {
            var registry = CreateValueConverterRegistry();
            iocProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueConverter>>(registry);
            iocProvider.RegisterSingleton<ICrossNamedInstanceRegistry<IMvxValueConverter>>(registry);
            iocProvider.RegisterSingleton<ICrossValueConverterLookup>(registry);
            iocProvider.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            FillValueConverters(registry);
        }

        protected virtual CrossValueConverterRegistry CreateValueConverterRegistry()
        {
            return new CrossValueConverterRegistry();
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("CommandParameter", new MvxCommandParameterValueConverter());
            registry.AddOrOverwrite("Language", new MvxLanguageConverter());
        }

        protected virtual void RegisterValueCombinerProvider(IMvxIoCProvider iocProvider)
        {
            var registry = CreateValueCombinerRegistry();
            iocProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueCombiner>>(registry);
            iocProvider.RegisterSingleton<ICrossNamedInstanceRegistry<IMvxValueCombiner>>(registry);
            iocProvider.RegisterSingleton<IMvxValueCombinerLookup>(registry);
            iocProvider.RegisterSingleton<IMvxValueCombinerRegistry>(registry);
            FillValueCombiners(registry);
        }

        protected virtual IMvxValueCombinerRegistry CreateValueCombinerRegistry()
        {
            return new MvxValueCombinerRegistry();
        }

        protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            // note that assembly based registration is not used here for efficiency reasons
            // - see #327 - https://github.com/slodge/MvvmCross/issues/327
            registry.AddOrOverwrite("Add", new MvxAddValueCombiner());
            registry.AddOrOverwrite("Divide", new MvxDivideValueCombiner());
            registry.AddOrOverwrite("Format", new MvxFormatValueCombiner());
            registry.AddOrOverwrite("If", new MvxIfValueCombiner());
            registry.AddOrOverwrite("Modulus", new MvxModulusValueCombiner());
            registry.AddOrOverwrite("Multiply", new MvxMultiplyValueCombiner());
            registry.AddOrOverwrite("Single", new MvxSingleValueCombiner());
            registry.AddOrOverwrite("Subtract", new MvxSubtractValueCombiner());
            registry.AddOrOverwrite("EqualTo", new MvxEqualToValueCombiner());
            registry.AddOrOverwrite("NotEqualTo", new MvxNotEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThanOrEqualTo", new MvxGreaterThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThan", new MvxGreaterThanValueCombiner());
            registry.AddOrOverwrite("LessThanOrEqualTo", new MvxLessThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("LessThan", new MvxLessThanValueCombiner());
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
            if (iocProvider.CanResolve<IMvxBindingParser>())
            {
                MvxBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            MvxBindingLog.Instance?.LogTrace("Registering Default Binding Parser");
            iocProvider.RegisterSingleton(CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxTibetBindingParser();
        }

        protected virtual void RegisterLanguageBindingParser(IMvxIoCProvider iocProvider)
        {
            if (iocProvider.CanResolve<IMvxLanguageBindingParser>())
            {
                MvxBindingLog.Instance?.LogTrace("Binding Parser already registered - so skipping Language parser");
                return;
            }
            MvxBindingLog.Instance?.LogTrace("Registering Language Binding Parser");
            iocProvider.RegisterSingleton(CreateLanguageBindingParser());
        }

        protected virtual IMvxLanguageBindingParser CreateLanguageBindingParser()
        {
            return new MvxLanguageBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser(IMvxIoCProvider iocProvider)
        {
            var parser = CreateBindingDescriptionParser();
            iocProvider.RegisterSingleton(parser);
        }

        private static ICrossBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser(IMvxIoCProvider iocProvider)
        {
            var tokeniser = CreateSourcePropertyPathParser();
            iocProvider.RegisterSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual IMvxSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new MvxSourcePropertyPathParser();
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
