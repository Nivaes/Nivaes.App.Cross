// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Views
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using MvvmCross;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Uap.Core;
    using MvvmCross.Platforms.Uap.Views.Suspension;
    using MvvmCross.ViewModels;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;

    public abstract class NivApplication
        : Microsoft.UI.Xaml.Application
    {
        protected IActivatedEventArgs ActivationArguments { get; private set; }

        protected Frame RootFrame { get; set; }

        protected NivApplication()
        {
            base.EnteredBackground += OnEnteredBackground;
            base.LeavingBackground += OnLeavingBackground;
            base.Suspending += OnSuspending;
            base.Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            base.OnLaunched(args);

            if (Window.Current == null)
            {
                var rootFrame = InitializeFrame();

                _ = RunAppStart(ActivationArguments);

                var window = new Microsoft.UI.Xaml.Window
                {
                    Content = rootFrame
                };

                DebugAttached();

                window.Activate();
            }
            else
            {
                if (Window.Current.Content is not Frame rootFrame)
                {
                    ActivationArguments = args.UWPLaunchActivatedEventArgs;

                    rootFrame = InitializeFrame(ActivationArguments);
                    Window.Current.Content = rootFrame;
                }

                DebugAttached();

                //if (true/*!ActivationArguments.PrelaunchActivated*/)
                //if (ActivationArguments.PreviousExecutionState == ApplicationExecutionState.NotRunning)
                if (args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
                {
                    Window.Current.Activate();

                    //if (rootFrame.Content == null)
                    //{
                    _ = RunAppStart(ActivationArguments);
                    //}

                    //var window = new Microsoft.UI.Xaml.Window
                    //{
                    //    Content = rootFrame
                    //};

                    //Window.Current.Activate();
                    //window.Activate();

                    //mWindow = window;
                }
            }
        }

        private void DebugAttached()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.BindingFailed += DebugSettings_BindingFailed;
            }
        }

        private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
        {

        }

        //protected override void OnWindowCreated(WindowCreatedEventArgs args)
        //{
        //    base.OnWindowCreated(args);
        //}

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            ActivationArguments = args;

            var rootFrame = InitializeFrame(args);

            _ = RunAppStart(args);

            Window.Current.Activate();
        }

        protected virtual async Task RunAppStart(IActivatedEventArgs activationArgs)
        {
            var instance = MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, ActivationArguments, nameof(Suspend));
            if (RootFrame?.Content == null)
            {
                await instance.EnsureInitialized().ConfigureAwait(false);

                if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                {
                    await startup.Start(GetAppStartHint(activationArgs)).ConfigureAwait(true);
                }
            }
            else
            {
                instance.PlatformSetup<MvxWindowsSetup>().UpdateActivationArguments(activationArgs);
            }
        }

        protected virtual object? GetAppStartHint(object? hint = null) => hint;

        /// <summary>
        /// Función probisional.
        /// </summary>
        protected virtual Frame InitializeFrame()
        {
            var rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;

            OnResumeFromTerminateState();

            RootFrame = rootFrame;

            return rootFrame;
        }

        protected virtual Frame InitializeFrame(IActivatedEventArgs activationArgs)
        {
            if (activationArgs == null) throw new ArgumentNullException(nameof(activationArgs));

            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                OnResumeFromTerminateState();
            }

            RootFrame = rootFrame;

            return rootFrame;
        }

        protected virtual void OnResumeFromTerminateState()
        {
        }

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            throw new MvxException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
        }

        protected virtual async void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var deferral = e.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await EnteringBackground(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual Task EnteringBackground(IMvxSuspensionManager suspensionManager)
        {
            if (suspensionManager == null) throw new ArgumentNullException(nameof(suspensionManager));

            return suspensionManager.SaveAsync();
        }

        protected virtual async void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var deferral = e.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await LeaveBackground(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual Task LeaveBackground(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }

        protected virtual Task Suspend(IMvxSuspensionManager suspensionManager)
        {
            if (suspensionManager == null) throw new ArgumentNullException(nameof(suspensionManager));

            return suspensionManager.SaveAsync();
        }

        protected virtual async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await Suspend(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual void OnResuming(object? sender, object e)
        {
            var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
            //_ = Resume(suspension).AsTask();
        }

        //protected virtual ValueTask Resume(IMvxSuspensionManager suspensionManager)
        //{
        //    return new ValueTask();
        //}
    }

    public class NivApplication<TMvxUapSetup, TApplication> : NivApplication
       where TMvxUapSetup : MvxWindowsSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        public NivApplication()
        {
            this.RegisterSetupType<TMvxUapSetup>();
        }
    }
}
