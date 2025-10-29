namespace Nivaes.App.Cross.Droid
{
    using System;
    using Nivaes.App.Cross.Droid.Presenters;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.IoC;

    public abstract class DroidCrossApplication : Application
    {
        protected DroidCrossApplication(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            container.AddDelegate<ICrossViewPresenter>((container) =>
            {
                return new DroidViewPresenter();
            });

            container.Merge(new DroidIoCServiceContainer());
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
