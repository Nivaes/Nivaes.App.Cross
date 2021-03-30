using Windows.ApplicationModel;

namespace Nivaes.App.Cross.Mobile.Windows.Sample
{
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using Nivaes.App.Cross.Mobele.Windows.Sample;
    using Nivaes.App.Cross.Mobile.Sample;
    using Nivaes.App.Cross.Views;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
        : AppParent
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            //AppCenter.Start("",
            //                typeof(Analytics), typeof(Crashes));

            this.InitializeComponent();
            //this.Suspending += OnSuspending;
        }

        ///// <summary>
        ///// Invoked when the application is launched normally by the end user.  Other entry points
        ///// will be used such as when the application is launched to open a specific file.
        ///// </summary>
        ///// <param name="e">Details about the launch request and process.</param>
        //protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
        //{
        //    if (e == null) throw new NullReferenceException(nameof(e));

        //    var window = new Microsoft.UI.Xaml.Window
        //    {

        //    };
        //    window.Activate();

        //    Frame? rootFrame = window.Content as Frame;

        //    //Frame? rootFrame = Window.Current.Content as Frame;

        //    // Do not repeat app initialization when the Window already has content,
        //    // just ensure that the window is active
        //    if (rootFrame == null)
        //    {
        //        // Create a Frame to act as the navigation context and navigate to the first page
        //        rootFrame = new Frame();

        //        rootFrame.NavigationFailed += OnNavigationFailed;

        //        //if (e.UWPLaunchActivatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
        //        //{
        //        //    //TODO: Load state from previously suspended application
        //        //}

        //        // Place the frame in the current Window
        //        //Window.Current.Content = rootFrame;
        //        rootFrame.Content = rootFrame;
        //    }

        //    //if (e.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
        //    //{
        //    //    if (rootFrame.Content == null)
        //    //    {
        //    //        // When the navigation stack isn't restored navigate to the first page,
        //    //        // configuring the new page by passing required information as a navigation
        //    //        // parameter
        //    //        rootFrame.Navigate(typeof(MainPage), e.Arguments);
        //    //    }
        //    //    // Ensure the current window is active
        //    //    Window.Current.Activate();
        //    //}
        //}

        //protected override Frame InitializeFrame(IActivatedEventArgs activationArgs)
        //{
        //    var window = new Microsoft.UI.Xaml.Window
        //    {
        //    };
        //    window.Activate();
        //    window.Content = new Frame();

        //    var rootFrame = window.Content as Frame;

        //    if (rootFrame == null)
        //    {

        //        rootFrame = CreateFrame();
        //        rootFrame.NavigationFailed += OnNavigationFailed;

        //        Window.Current.Content = rootFrame;
        //        rootFrame.Content = rootFrame;
        //    }

        //     ToDo: Review. UWPLaunchActivatedEventArgs lauch a exception with Microsoft.WinUI 3.0.0-preview2.200713.0
        //    if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
        //    {
        //        OnResumeFromTerminateState();
        //    }

        //    RootFrame = rootFrame;

        //    return rootFrame;
        //}

        ///// <summary>
        ///// Invoked when Navigation to a certain page fails
        ///// </summary>
        ///// <param name="sender">The Frame which failed navigation</param>
        ///// <param name="e">Details about the navigation failure</param>
        //void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        //{
        //    throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        //}

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }

    public abstract class AppParent
        : NivApplication<Setup, MobileSampleCrossApplication<AppMobileSampleAppStart>>
    {
    }
}
