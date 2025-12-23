namespace Playground.iOS
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Platforms.Ios.Core;
    using MvvmCross.Plugin;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit;
    using Playground.Core;
    using Playground.iOS.Bindings;
    using Playground.iOS.Controls;
    using Serilog;
    using Serilog.Extensions.Logging;

    [RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class Setup : MvxIosSetup<App>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Async(a => a.Trace())
                .WriteTo.Async(a => a.NSLog())
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<BinaryEdit>(
                "MyCount",
                (arg) => new BinaryEditTargetBinding(arg));

            base.FillTargetFactories(registry);
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
        }
    }
}
