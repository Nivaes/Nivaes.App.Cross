namespace Nivaes.App.Cross.UIKit
{
    using Foundation;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.UIKit.Presenters;
    using Nivaes.IoC;

    public class CrossApplicationDelegate 
        : UIApplicationDelegate
    {
        public override UIWindow? Window { get; set; }

        public CrossApplicationDelegate()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddDelegate<IViewPresenter>((container) =>
            {
                return new UIKitViewPresenter();
            });
            container.Merge(new UIKitSubcontainer());
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window ??= new UIWindow(UIScreen.MainScreen.Bounds);

            RunAppStart(launchOptions);

            Window.MakeKeyAndVisible();

            return true;
        }

        protected virtual void RunAppStart(object? hint = null)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddInstance(new AppDataModel(Window!));

            //if (RootFrame!.Content == null)
            //{
                var application = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ICrossApplication>();

                if (application != null)
                {
                    application.ApplicationStart.NavigateToFirstViewModel();
                    //startup.Start(GetAppStartHint(arguments));
                }
            //}
        }
    }
}
