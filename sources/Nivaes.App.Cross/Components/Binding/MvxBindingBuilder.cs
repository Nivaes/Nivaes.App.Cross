namespace MvvmCross.Binding
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding.Bindings.SourceSteps;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.IoC;
    using MvvmCross.Logging;
    using Nivaes.App.Cross;

    public class MvxBindingBuilder : MvxCoreBindingBuilder
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
            iocProvider.RegisterSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            iocProvider.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);
        }

        protected virtual void FillSourceStepFactory(IMvxSourceStepFactoryRegistry registry)
        {
            registry.AddOrOverwrite(typeof(MvxCombinerSourceStepDescription), new MvxCombinerSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxLiteralSourceStepDescription), new MvxLiteralSourceStepFactory());
        }

        protected virtual IMvxSourceStepFactoryRegistry CreateSourceStepFactoryRegistry()
        {
            return new MvxSourceStepFactory();
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
                MvxLogHost.Default?.Log(LogLevel.Trace, "source binding factory extension host not provided - so no source extensions will be used");
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
            iocProvider.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            iocProvider.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
        }

        protected virtual IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new MvxTargetBindingFactoryRegistry();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }
    }
}
