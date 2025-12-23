namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;
    using MvvmCross.Logging;

    public class CrossBindingBuilder 
        : CrossCoreBindingBuilder
    {
        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public override void DoRegistration(IMvxIoCProvider iocProvider)
        {
            base.DoRegistration(iocProvider);
            RegisterBindingFactories(iocProvider);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void RegisterBindingFactories(IMvxIoCProvider iocProvider)
        {
            RegisterMvxBindingFactories(iocProvider);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected virtual void RegisterMvxBindingFactories(IMvxIoCProvider iocProvider)
        {
            RegisterSourceStepFactory(iocProvider);
            RegisterSourceFactory(iocProvider);
            RegisterTargetFactory(iocProvider);
        }

        protected virtual void RegisterSourceStepFactory(IMvxIoCProvider iocProvider)
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            iocProvider.RegisterSingleton<ICrossSourceStepFactoryRegistry>(sourceStepFactory);
            iocProvider.RegisterSingleton<ICrossSourceStepFactory>(sourceStepFactory);
        }

        protected virtual void FillSourceStepFactory(ICrossSourceStepFactoryRegistry registry)
        {
            registry.AddOrOverwrite(typeof(CrossCombinerSourceStepDescription), new CrossCombinerSourceStepFactory());
            registry.AddOrOverwrite(typeof(CrossPathSourceStepDescription), new CrossPathSourceStepFactory());
            registry.AddOrOverwrite(typeof(CrossLiteralSourceStepDescription), new CrossLiteralSourceStepFactory());
        }

        protected virtual ICrossSourceStepFactoryRegistry CreateSourceStepFactoryRegistry()
        {
            return new CrossSourceStepFactory();
        }

        protected virtual void RegisterSourceFactory(IMvxIoCProvider iocProvider)
        {
            var sourceFactory = CreateSourceBindingFactory();
            iocProvider.RegisterSingleton<ICrossSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as ICrossSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                iocProvider.RegisterSingleton<ICrossSourceBindingFactoryExtensionHost>(extensionHost);
            }
            else
                CrossLogHost.Default?.Log(LogLevel.Trace, "source binding factory extension host not provided - so no source extensions will be used");
        }

        protected virtual void RegisterSourceBindingFactoryExtensions(ICrossSourceBindingFactoryExtensionHost extensionHost)
        {
            extensionHost.Extensions.Add(new CrossPropertySourceBindingFactoryExtension());
        }

        protected virtual ICrossSourceBindingFactory CreateSourceBindingFactory()
        {
            return new CrossSourceBindingFactory();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected virtual void RegisterTargetFactory(IMvxIoCProvider iocProvider)
        {
            var targetRegistry = CreateTargetBindingRegistry();
            FillTargetFactories(targetRegistry);
            iocProvider.RegisterSingleton<ICrossTargetBindingFactoryRegistry>(targetRegistry);
            iocProvider.RegisterSingleton<ICrossTargetBindingFactory>(targetRegistry);
        }

        protected virtual ICrossTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new CrossTargetBindingFactoryRegistry();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected virtual void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }
    }
}
