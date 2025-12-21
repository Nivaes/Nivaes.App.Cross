using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Diagnostics.CodeAnalysis;

    using MvvmCross.Core;
    using MvvmCross.Platforms.Android.Core;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;


    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class MvxAndroidApplication : Application, IMvxAndroidApplication
    {
        public static MvxAndroidApplication Instance { get; private set; }

        protected MvxAndroidApplication()
        {
            Instance = this;
            RegisterSetup();
        }

        protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
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
    public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
        where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
        where TApplication : class, ICrossApplication, new()
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
}