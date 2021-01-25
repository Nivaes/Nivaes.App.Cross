// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.Runtime;

namespace Nivaes.App.Cross.Droid.ViewModels
{
    using System;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.Droid.Views;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication>
        : Application, IMvxAndroidApplication
            where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
            where TApplication : class, ICrossApplication, new()
    {
        public IMvxAndroidSetup Setup { get; }

        //public static MvxAndroidApplication Instance { get; private set; }

        //protected MvxAndroidApplication()
        //{
        //    //Instance = this;
        //    //RegisterSetup();
        //}

        protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            //Instance = this;
            //RegisterSetup();
            Setup = new TMvxAndroidSetup();
        }

        public override void OnCreate()
        {
            //_ = mSetup.StartSetupInitialization();

            base.OnCreate();
        }

        public override void RegisterActivityLifecycleCallbacks(IActivityLifecycleCallbacks? callback)
        {
            base.RegisterActivityLifecycleCallbacks(callback);
        }

        public override void UnregisterActivityLifecycleCallbacks(IActivityLifecycleCallbacks? callback)
        {
            base.UnregisterActivityLifecycleCallbacks(callback);
        }

        public override void RegisterComponentCallbacks(global::Android.Content.IComponentCallbacks? callback)
        {
            base.RegisterComponentCallbacks(callback);
        }

        public override void UnregisterComponentCallbacks(global::Android.Content.IComponentCallbacks? callback)
        {
            base.UnregisterComponentCallbacks(callback);
        }

        public override void OnConfigurationChanged(global::Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        //protected override void RegisterSetup()
        //{
        //    //this.RegisterSetupType<TMvxAndroidSetup>();
        //}
    }
}
