// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;

namespace Nivaes.App.Cross.Droid;

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class CrossAndroidApplication : Application, IMvxAndroidApplication
{
    public static CrossAndroidApplication Instance { get; private set; }

    protected CrossAndroidApplication()
    {
        Instance = this;
        RegisterSetup();
    }

    protected CrossAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        Instance = this;
        RegisterSetup();
    }

    protected abstract void RegisterSetup();

    public override void OnCreate()
    {
        base.OnCreate();

        CrossAndroidSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
    }

    protected virtual void RunAppStart()
    {
        if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true && !startup.IsStarted)
        {
            startup.Start();
        }
    }
}

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication> : CrossAndroidApplication
  where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
  where TApplication : class, IMvxApplication, new()
{
    protected MvxAndroidApplication() : base()
    {
    }

    protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected override void RegisterSetup()
    {
        this.RegisterSetupType<TMvxAndroidSetup>();
    }
}
