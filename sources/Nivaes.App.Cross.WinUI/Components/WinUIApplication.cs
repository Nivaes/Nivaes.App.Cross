namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Intrinsics.X86;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using Nivaes.IoC;

    public abstract class WinUIApplication : Application
    {
        protected Frame RootFrame { get; set; }
        public Window MainWindow { get; protected set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected WinUIApplication()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            InitializeFrame();

            RunAppStart(args.Arguments);

            MainWindow.Activate();
        }

        protected virtual void RunAppStart(string arguments)
        {
            //var instance = MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, arguments, "Suspend");

            if (RootFrame.Content == null)
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
            if (MainWindow == null)
            {
                MainWindow = CreateWindow();
            }

            var rootFrame = MainWindow.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                MainWindow.Content = rootFrame;
            }

            RootFrame = rootFrame;
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
    }
}
