namespace Nivaes.App.Cross.AppKit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Foundation;
    using Nivaes.App.Cross.AppKit.Presenters;
    using Nivaes.IoC;

    public class CrossApplicationDelegate : NSViewController
    {
        //public override UIWindow? Window { get; set; }

        //public CrossApplicationDelegate()
        //{
        //    var container = Singleton<CrossIoCServiceContainer>.Instance;
        //    container.AddDelegate<IViewPresenter>((container) =>
        //    {
        //        return new UIKitViewPresenter();
        //    });
        //    container.Merge(new UIKitSubcontainer());
        //}

        //public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        //{
        //    Window ??= new UIWindow(UIScreen.MainScreen.Bounds);

        //    RunAppStart(launchOptions);

        //    Window.MakeKeyAndVisible();

        //    return true;
        //}

        //protected virtual void RunAppStart(object? hint = null)
        //{
        //    var container = Singleton<CrossIoCServiceContainer>.Instance;
        //    container.AddInstance(new AppDataModel());

        //    //if (RootFrame!.Content == null)
        //    //{
        //        var application = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ICrossApplication>();

        //        if (application != null)
        //        {
        //            application.ApplicationStart.NavigateToFirstViewModel();
        //            //startup.Start(GetAppStartHint(arguments));
        //        }
        //    //}
        //}
    }
}
