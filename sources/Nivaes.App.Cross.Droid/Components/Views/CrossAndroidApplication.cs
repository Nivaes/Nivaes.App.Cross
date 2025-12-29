using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Nivaes.App.Cross;
using Nivaes.App.Cross.Droid;
using Nivaes.IoC;

namespace Nivaes.App.Cross.Droid;

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class CrossAndroidApplication 
    : Application, IMvxAndroidApplication
{
    public static CrossAndroidApplication? Instance { get; private set; }

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

        MvxAndroidSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
    }

    protected virtual void RunAppStart()
    {
        if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true && !startup.IsStarted)
        {
            startup.Start();
        }
    }
}

[RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
public abstract class CrossAndroidApplication<TMvxAndroidSetup, TApplication> : CrossAndroidApplication
    where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
    where TApplication : class, ICrossApplication, new()
{
    protected CrossAndroidApplication() : base()
    {
    }

    protected CrossAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    protected override void RegisterSetup()
    {
        this.RegisterSetupType<TMvxAndroidSetup>();
    }
}