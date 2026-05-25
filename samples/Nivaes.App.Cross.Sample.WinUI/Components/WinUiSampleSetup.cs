//using Microsoft.Extensions.Logging;
//using Nivaes.App.Cross.WinUI;
//using Serilog;
//using Serilog.Extensions.Logging;

//namespace Nivaes.App.Cross.Sample.WinUI;

//public class WinUiSampleSetup 
//    : MvxWindowsSetup<Nivaes.App.Cross.Sample.SampleApp>
//{
//    protected override void InitializeViewLookup()
//    {
//        // ToDo: Cargar esto con roslyn
//        var viewsManager = new CrossViewsManager(new[] {
//                    CrossViewsManager.New<RootViewModel, RootView>(),
//                    CrossViewsManager.New<NewWindowViewModel, NewWindow>(),
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
//            .CreateLogger();

//        return new SerilogLoggerFactory();
//    }
//}
