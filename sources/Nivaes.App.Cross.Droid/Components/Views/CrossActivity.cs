namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.Content;
    using Android.Runtime;
    using Android.Views;

    [Register("nivaes.app.cross.CrossActivity")]
    public abstract class CrossActivity<TViewModel>
        : Activity, IView, IBindingView
        where TViewModel : class, IViewModel
    {

        protected const int NoContent = 0;

        private readonly int _resourceId;
        private Bundle? _bundle;

        #region Constructors
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
            _bundle = Intent?.Extras;

            if (_bundle != null)
            {
                var key = _bundle.GetInt("viewModelKey");
                if (Singleton<TemporaryStore<IViewModel>>.Instance.TryGetAndRemove(key, out var viewModel))
                {
                    ViewModel = (TViewModel?)viewModel;
                }
            }

            base.OnCreate(savedInstanceState);

            if (_resourceId != NoContent)
            {
                try
                {
                    var content = LayoutInflater.Inflate(_resourceId, null);
                    base.SetContentView(content);
                }
                catch (Exception ex)
                {
                    throw new CrossException("Error al generar el recurso", ex);
                }
            }

            Binding();
        }

        //protected override void AttachBaseContext(Context? @base)
        //{
        //    base.AttachBaseContext(new CrossContextWrapper(@base));
        //}

        protected abstract void Binding();


        //public override void SetContentView(int layoutResID)
        //{
        //    if (BaseContextToAttach(this) is MvxContextWrapper)
        //    {
        //        var view = this.BindingInflate(layoutResID, null);
        //        base.SetContentView(view);
        //        return;
        //    }

        //    base.SetContentView(layoutResID);
        //}

        //public override View? OnCreateView(string name, Context context, IAttributeSet attrs)
        //{
        //    var aa = base.OnCreateView(name, context, attrs);
        //    return aa;
        //}

        //public override View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
        //{
        //    var aa = base.OnCreateView(parent, name, context, attrs);
        //    return aa;
        //}
        #endregion

        #region Properties
        private TViewModel? mViewModel;

        public TViewModel? ViewModel
        {
            [System.Diagnostics.DebuggerStepThrough]
            get => mViewModel;
            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                mViewModel = value;
            }
        }
        #endregion
    }
}
