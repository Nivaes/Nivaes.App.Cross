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

        public override void WillEnterForeground(UIApplication application)
        {
            base.WillEnterForeground(application);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            base.DidEnterBackground(application);
        }

        public override void WillTerminate(UIApplication application)
        {
            base.WillTerminate(application);
        }

        public override void WillEncodeRestorableState(UIApplication application, NSCoder coder)
        {
            base.WillEncodeRestorableState(application, coder);
        }

        public override bool WillContinueUserActivity(UIApplication application, string userActivityType)
        {
            return base.WillContinueUserActivity(application, userActivityType);
        }

        //public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
        //{
        //    return base.WillFinishLaunching(application, launchOptions);
        //}

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
