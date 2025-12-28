namespace Nivaes.App.Cross.Sample
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;
    using Playground.Droid.Bindings;
    using Playground.Droid.Controls;
    using Serilog;
    using Serilog.Extensions.Logging;

    [RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class Setup : MvxAndroidSetup<App>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies =>
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(MvxRecyclerView).Assembly
            };

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

            pluginManager.EnsurePluginLoaded<Nivaes.App.Cross.Droid.Plugin>();
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(a => a.AndroidLog())
                .WriteTo.Async(a => a.Trace())
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
