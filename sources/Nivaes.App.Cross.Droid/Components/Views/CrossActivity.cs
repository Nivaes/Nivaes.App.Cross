namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Runtime;
    using Android.Views;

    [Register("nivaes.app.cross.CrossActivity")]
    public abstract class CrossActivity<TViewModel>
        : Activity, IView
        where TViewModel : class, IViewModel
    {

        protected const int NoContent = 0;

        private readonly int _resourceId;
        private Bundle? _bundle;

        protected CrossActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected CrossActivity(IntPtr javaReference, JniHandleOwnership transfer)
           : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            _bundle = savedInstanceState;

            base.OnCreate(savedInstanceState);

            if (_resourceId != NoContent)
            {
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }
    }
}
