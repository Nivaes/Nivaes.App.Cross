// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace MvvmCross.Android.Views
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Droid;

    [Activity(
        Label = "@string/app_name"
        , MainLauncher = true
        , Icon = "@mipmap/ic_launcher"
        , RoundIcon = "@mipmap/ic_launcher_round"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true)]
    [Register("nivaes.app.cross.SplashScreenActivity")]
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

        protected override void OnCreate(Bundle bundle)
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
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            Task.Run(async () => await Initialize(setup).ConfigureAwait(false));

            base.OnResume();
        }

        protected async ValueTask RunAppStar(Bundle? bundle)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                await startup.StartAsync(GetAppStartHint(bundle)).ConfigureAwait(false);
            }
            Finish();
        }

        private async ValueTask Initialize(MvxAndroidSetupSingleton setup)
        {
            await setup.EnsureInitialized().ConfigureAwait(false);

            await RunAppStar(mBundle).ConfigureAwait(false);
        }

        //protected override void OnPause()
        //{
        //    base.OnPause();
        //}

        //protected virtual async Task RunAppStartAsync(Bundle bundle)
        //{
        //    if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup))
        //    {
        //        if(!startup.IsStarted)
        //        {
        //            await startup.StartAsync(GetAppStartHint(bundle)).ConfigureAwait(false);
        //        }
        //        else
        //        {
        //            Finish();
        //        }
        //    }
        //}

        protected object? GetAppStartHint(object? hint = null)
        {
            return hint;
        }
    }

    //public abstract class MvxSplashScreenActivity<TMvxAndroidSetup, TApplication> : SplashScreenActivity
    //        where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
    //        where TApplication : class, IMvxApplication, new()
    //{
    //    protected MvxSplashScreenActivity(int resourceId = NoContent) : base(resourceId)
    //    {
    //    }

    //    protected override void RegisterSetup()
    //    {
    //        this.RegisterSetupType<TMvxAndroidSetup>();
    //    }
    //}
}
