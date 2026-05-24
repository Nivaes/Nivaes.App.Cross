//using System.Diagnostics.CodeAnalysis;
//using Microsoft.Extensions.Logging;
//using Nivaes.App.Cross;
//using Nivaes.App.Cross.UIKitOS;
//using Playground.iOS.Bindings;
//using Playground.iOS.Controls;
//using Serilog;
//using Serilog.Extensions.Logging;

//namespace Nivaes.App.Cross.Sample.UIKitOS;

//[RequiresUnreferencedCode("Uses MvvmCross reflection based plugin loading")]
//#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
//public class Setup : MvxIosSetup<SampleApp>
//#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
//{
//    protected override void InitializeViewLookup()
//    {
//        // ToDo: Cargar esto con roslyn
//        var viewsManager = new CrossViewsManager(new[] {
//                    CrossViewsManager.New<RootViewModel, RootView>(),
//                    CrossViewsManager.New<ChildViewModel, ChildView>(),
//                });

//        Singleton<CrossViewsManager>.Add(viewsManager);
//    }

//    protected override ILoggerProvider CreateLogProvider()
//    {
//        return new SerilogLoggerProvider();
//    }

//    protected override ILoggerFactory CreateLogFactory()
//    {
//        Log.Logger = new LoggerConfiguration()
//            .MinimumLevel.Debug()
//            .WriteTo.Async(a => a.Trace())
//            .WriteTo.Async(a => a.NSLog())
//            .CreateLogger();

//        return new SerilogLoggerFactory();
//    }

//    protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
//    {
//        registry.RegisterCustomBindingFactory<BinaryEdit>(
//            "MyCount",
//            (arg) => new BinaryEditTargetBinding(arg));

//        base.FillTargetFactories(registry);
//    }

//    [Obsolete("Cargar los plugins de otra manera")]
//    public override void LoadPlugins(IMvxPluginManager pluginManager)
//    {
//        base.LoadPlugins(pluginManager);
//        pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Json.Plugin>();
//    }
//}
