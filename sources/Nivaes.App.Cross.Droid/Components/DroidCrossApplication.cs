namespace Nivaes.App.Cross.Droid
{
    using System;
    using Nivaes.IoC;

    [Obsolete()]
    public abstract class DroidCrossApplication 
        : Application
    {
        protected DroidCrossApplication(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;

            container.AddDelegate<ICrossViewPresenter>((container) =>
            {
                return new CrossDroidViewPresenter();
            });

            container.Merge(new DroidIoCServiceContainer());
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
