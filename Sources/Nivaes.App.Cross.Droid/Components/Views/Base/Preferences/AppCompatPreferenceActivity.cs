using Android.Preferences;
using Android.Runtime;
using AndroidX.AppCompat.App;
using ActionBar = AndroidX.AppCompat.App.ActionBar;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>
    ///  A {@link android.preference.PreferenceActivity} which implements and proxies the necessary callsto be used with AppCompat.
    /// </summary>
    [Register("nivaes.app.AppCompatPreferenceActivity")]
    public class AppCompatPreferenceActivity : PreferenceActivity
    {
        public AppCompatPreferenceActivity()
            : base()
        {
        }

        protected AppCompatPreferenceActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private AppCompatDelegate mDelegate;

        private AppCompatDelegate Delegate
        {
            get
            {
                if (mDelegate == null)
                {
                    mDelegate = AppCompatDelegate.Create(this, null);
                }
                return mDelegate;
            }
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            //Delegate.InstallViewFactory();
            //Delegate.OnCreate(savedInstanceState);

            base.OnCreate(savedInstanceState);
        }

        //protected override void OnPostCreate(Bundle savedInstanceState)
        //{
        //    base.OnPostCreate(savedInstanceState);
        //    Delegate.OnPostCreate(savedInstanceState);
        //}

        public ActionBar SupportActionBar => Delegate.SupportActionBar;

        public void SetSupportActionBar(Toolbar toolbar) => Delegate.SetSupportActionBar(toolbar);

        //public override MenuInflater MenuInflater => Delegate.MenuInflater;

        //public override void SetContentView(int layoutResID) => Delegate.SetContentView(layoutResID);

        //public override void SetContentView(View view, ViewGroup.LayoutParams @params) => Delegate.SetContentView(view, @params);

        //public override void SetContentView(View view) => base.SetContentView(view);

        //public override void AddContentView(View view, ViewGroup.LayoutParams @params) => Delegate.AddContentView(view, @params);

        //protected override void OnPostResume()
        //{
        //    base.OnPostResume();
        //    Delegate.OnPostResume();
        //}

        //protected override void OnTitleChanged(ICharSequence title, Color color)
        //{
        //    base.OnTitleChanged(title, color);
        //    Delegate.SetTitle(title);
        //}

        //public override void OnConfigurationChanged(Configuration newConfig)
        //{
        //    base.OnConfigurationChanged(newConfig);
        //    Delegate.OnConfigurationChanged(newConfig);
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //    Delegate.OnStop();
        //}

        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    Delegate.OnDestroy();
        //}

        //public override void InvalidateOptionsMenu()
        //{
        //    Delegate.InvalidateOptionsMenu();
        //}
    }
}
