namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Runtime;

    [Register("nivaes.app.cross.CrossActivity")]
    public abstract class CrossActivity<TViewModel>
        : Activity, IView
        where TViewModel : class, IViewModel
    {
        protected CrossActivity(IntPtr javaReference, JniHandleOwnership transfer)
           : base(javaReference, transfer)
        {
        }

        protected CrossActivity()
        {
            //BindingContext = new MvxAndroidBindingContext(this, this);
            //this.AddEventListeners();
        }
    }
}
