namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using Nivaes.App.Cross;
    using Nivaes.IoC;

    public abstract class CrossWinUIApplication 
        : Application
    {
        protected Frame? RootFrame { get; set; }
        public Window? MainWindow { get; protected set; }

        protected CrossWinUIApplication()
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            container.AddDelegate<ICrossViewPresenter>((container) =>
            {
                return new CrossWinUIViewPresenter();
            });

            container.Merge(new WinUISubcontainer());

            base.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            InitializeFrame();

            RunAppStart(args.Arguments);

            MainWindow!.Activate();
        }

        protected virtual void RunAppStart(string arguments)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddInstance(new AppDataModel(RootFrame!));

            if (RootFrame!.Content == null)
            {
                var application = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ICrossApplication>();

                if (application != null)
                {
                    application.ApplicationStart.NavigateToFirstViewModel();
                    //startup.Start(GetAppStartHint(arguments));
                }
            }
        }

        //protected virtual object? GetAppStartHint(object? hint = null)
        //{
        //    return hint;
        //}

       private void InitializeFrame()
        {
            if (this.MainWindow == null)
            {
                this.MainWindow = CreateWindow();
            }

            var rootFrame = this.MainWindow.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                this.MainWindow.Content = rootFrame;
            }

            this.RootFrame = rootFrame;
        }

        protected virtual Window CreateWindow()
        {
            return new Window();
        }

        protected virtual Frame CreateFrame()
        {
            return new Frame();
        }

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new CrossException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
        }

        protected virtual void RegisterSetup()
        {

        }

        private void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Log and handle exceptions as appropriate.
            // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        }
    }
}
