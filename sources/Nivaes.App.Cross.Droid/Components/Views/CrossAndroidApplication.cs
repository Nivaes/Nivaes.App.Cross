namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Runtime;

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class CrossAndroidApplication : Application, ICrossAndroidApplication
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
            throw new NotImplementedException();
            //if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true && !startup.IsStarted)
            //{
            //    startup.Start();
            //}
        }
    }

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class MvxAndroidApplication<TCrossAndroidSetup, TApplication> : CrossAndroidApplication
      where TCrossAndroidSetup : MvxAndroidSetup<TApplication>, new()
      where TApplication : class, ICrossApplication, new()
    {
        protected MvxAndroidApplication() : base()
        {
        }

        protected MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [Obsolete("No compatible con AoT")]
        protected override void RegisterSetup()
        {
            throw new NotImplementedException();
            //this.RegisterSetupType<CrossAndroidSetup>();
        }
    }
}