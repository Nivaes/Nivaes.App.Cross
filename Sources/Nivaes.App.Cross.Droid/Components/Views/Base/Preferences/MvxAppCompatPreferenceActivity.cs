//using Android.Content;
//using Android.Runtime;
//using Android.Util;
//using Android.Views;

//namespace Nivaes.App.Cross.Droid
//{
//    //ToDo: Deduplicar 
//    [Register("nivaes.app.MvxAppCompatPreferenceActivity")]
//    public class MvxAppCompatPreferenceActivity
//        : MvxEventSourceAppCompatPreferenceActivity, IMvxAndroidView
//    {
//        private View? _view;

//        protected MvxAppCompatPreferenceActivity()
//        {
//            BindingContext = new MvxAndroidBindingContext(this, this);
//            this.AddEventListeners();
//        }

//        protected MvxAppCompatPreferenceActivity(IntPtr javaReference, JniHandleOwnership transfer)
//            : base(javaReference, transfer)
//        {
//        }

//        public object? DataContext
//        {
//            get => BindingContext.DataContext;
//            set => BindingContext.DataContext = value;
//        }

//        public ICrossViewModel? ViewModel
//        {
//            get => (ICrossViewModel?)DataContext;
//            set
//            {
//                DataContext = value;
//                OnViewModelSet();
//            }
//        }

//        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
//        {
//            StartActivityForResult(intent, requestCode);
//        }

//        protected virtual void OnViewModelSet()
//        {
//        }

//        public ICrossBindingContext BindingContext { get; set; }

//        public override void SetContentView(int layoutResId)
//        {
//            _view = this.BindingInflate(layoutResId, null);

//            SetContentView(_view);
//        }

//        protected override void AttachBaseContext(Context? @base)
//        {
//            //if (this is IMvxAndroidSplashScreenActivity)
//            //{
//            //    // Do not attach our inflater to splash screens.
//            //    base.AttachBaseContext(@base);
//            //    return;
//            //}
//            base.AttachBaseContext(CrossContextWrapper.Wrap(@base, this));
//        }

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);
//            ViewModel?.ViewCreated();
//        }

//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//            ViewModel?.ViewDestroy(IsFinishing);
//        }

//        protected override void OnStart()
//        {
//            base.OnStart();
//            ViewModel?.ViewAppearing();
//        }

//        protected override void OnResume()
//        {
//            base.OnResume();
//            ViewModel?.ViewAppeared();
//        }

//        protected override void OnPause()
//        {
//            base.OnPause();
//            ViewModel?.ViewDisappearing();
//        }

//        protected override void OnStop()
//        {
//            base.OnStop();
//            ViewModel?.ViewDisappeared();
//        }

//        public override View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
//        {
//            //if (System.Diagnostics.Debugger.IsAttached)
//                System.Diagnostics.Debugger.Break();

//            //var view = MvxAppCompatActivityHelper.OnCreateView(parent, name, context, attrs);
//            //return view ?? base.OnCreateView(parent, name, context, attrs);
//            return null;
//        }
//    }
//}
