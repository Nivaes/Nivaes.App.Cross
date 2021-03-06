﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Droid.Views;
    using Nivaes.App.Cross.ViewModels;

    [Activity(
        Label = "@string/app_name"
        , MainLauncher = true
        , Icon = "@mipmap/ic_launcher"
        , RoundIcon = "@mipmap/ic_launcher_round"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true)]
    [Register("nivaes.app.SplashScreen")]
    [Android.Runtime.Preserve(AllMembers = true)]
    public class SplashScreenActivity
        : Activity
    {
        protected virtual int ResourceId => Resource.Layout.splash_screen;

        private Bundle? mBundle;

        public SplashScreenActivity()
        {
        }

        protected SplashScreenActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle? bundle)
        {
            base.RequestWindowFeature(WindowFeatures.NoTitle);

            mBundle = bundle;

            //var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            //setup.InitializeAndMonitor(this);

            base.OnCreate(bundle);

            // Set our view from the "splash" layout resource
            // Be careful to use non-binding inflation
            var content = LayoutInflater.Inflate(ResourceId, null);
            base.SetContentView(content);
        }

        protected override void OnResume()
        {
            //var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext!);

            //_ = Initialize(setup);

            _ = Initialize();

            base.OnResume();
        }

        //private async Task Initialize(MvxAndroidSetupSingleton setup)
        //{
        //    await setup.EnsureInitialized().ConfigureAwait(false);

        //    await RunAppStar(mBundle).ConfigureAwait(false);
        //}

        private async Task Initialize()
        {
            var application = base.ApplicationContext as IMvxAndroidApplication;

            await application!.Setup.StartSetupInitialization().ConfigureAwait(false);

            //await setup.EnsureInitialized().ConfigureAwait(false);

            await RunAppStar(mBundle).ConfigureAwait(false);
        }

        protected async ValueTask RunAppStar(Bundle? bundle)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                await startup.Start(bundle).ConfigureAwait(false);
            }
            base.Finish();
        }
    }
}
