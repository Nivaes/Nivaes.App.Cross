// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views
{
    using System;
    using MvvmCross.Core;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.ViewModels;

    public abstract class MvxAndroidApplication
        : Application, IMvxAndroidApplication
    {
        public static MvxAndroidApplication Instance { get; private set; }

        public MvxAndroidApplication()
        {
            Instance = this;
            RegisterSetup();
        }

        public MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Instance = this;
            RegisterSetup();
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
      where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
      where TApplication : class, IMvxApplication, new()
    {
        public MvxAndroidApplication()
            : base()
        {
        }

        public MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
