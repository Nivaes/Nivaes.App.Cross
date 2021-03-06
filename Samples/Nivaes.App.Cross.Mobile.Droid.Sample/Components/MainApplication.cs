﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using System;
    using Android.App;
    using Android.Runtime;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using Nivaes.App.Cross.Droid.ViewModels;
    using Nivaes.App.Cross.Mobile.Sample;

    [Application]
    public sealed class MainApplication
        : MvxAndroidApplication<Setup, MobileSampleCrossApplication<AppMobileSampleAppStart>>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            AppCenter.Start("",
               typeof(Analytics), typeof(Crashes));

            base.OnCreate();

            Xamarin.Essentials.Platform.Init(this);
        }

        //protected override void OnCreate(Bundle? savedInstanceState)
        //{

        //    base.OnCreate(savedInstanceState);

        //    Xamarin.Essentials.Platform.Init(this, savedInstanceState);

        //    // Set our view from the "main" layout resource
        //    SetContentView(Resource.Layout.activity_main);
        //}
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
    }
}
