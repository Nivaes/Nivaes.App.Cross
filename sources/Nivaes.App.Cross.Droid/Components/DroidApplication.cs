namespace Nivaes.App.Cross.Droid
{
    using System;
    using Nivaes.App.Cross.Droid.Presenters;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.IoC;

    public abstract class DroidApplication : Application
    {
        protected DroidApplication(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            container.AddDelegate<IViewPresenter>((container) =>
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
