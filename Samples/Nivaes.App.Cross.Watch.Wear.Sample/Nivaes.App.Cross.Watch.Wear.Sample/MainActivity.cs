﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Watch.Wear.Sample
{
    using Android.App;
    using Android.OS;
    using Android.Support.Wearable.Activity;
    using Android.Widget;

    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity
        : WearableActivity
    {
        protected override void OnCreate(Bundle? bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            _ = FindViewById<TextView>(Resource.Id.text);
            SetAmbientEnabled();
        }
    }
}


