// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Views
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;
    using MvvmCross.Core;
    using MvvmCross.Exceptions;
    using MvvmCross.Platforms.Uap.Core;
    using MvvmCross.Platforms.Uap.Views.Suspension;
    using MvvmCross.ViewModels;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;

    public abstract class Application : Microsoft.UI.Xaml.Application
    {
        protected IActivatedEventArgs ActivationArguments { get; private set; }

        protected Frame? RootFrame { get; set; }

        //private Window? mWindow;

        protected Application()
        {
            RegisterSetup();
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
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            base.OnLaunched(args);

            if (!(Window.Current.Content is Frame rootFrame))
            {
                ActivationArguments = args.UWPLaunchActivatedEventArgs;

                rootFrame = InitializeFrame(ActivationArguments);
                Window.Current.Content = rootFrame;
            }

            //if (true/*!ActivationArguments.PrelaunchActivated*/)
            //if (ActivationArguments.PreviousExecutionState == ApplicationExecutionState.NotRunning)
            if(args.UWPLaunchActivatedEventArgs.PrelaunchActivated == false)
            {
                Window.Current.Activate();

                //if (rootFrame.Content == null)
                //{
                await RunAppStart(ActivationArguments).ConfigureAwait(false);
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

        //protected override void OnWindowCreated(WindowCreatedEventArgs args)
        //{
        //    base.OnWindowCreated(args);
        //}

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            ActivationArguments = args;

            //mWindow!.Content = InitializeFrame(args);
            var rootFrame = InitializeFrame(args);

            await RunAppStart(args).ConfigureAwait(false);

            Window.Current.Activate();
            //mWindow!.Activate();
        }

        protected virtual async ValueTask RunAppStart(IActivatedEventArgs activationArgs)
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

        protected virtual object GetAppStartHint(object hint = null) => hint;

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
            _ = Resume(suspension);
        }

        protected virtual Task Resume(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public class Application<TMvxUapSetup, TApplication> : Application
       where TMvxUapSetup : MvxWindowsSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxUapSetup>();
        }
    }
}
