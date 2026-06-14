using Android.Content;
using Android.Runtime;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;

namespace Nivaes.App.Cross.Droid
{
    [Register("nivaes.cross.eventSourceActivity")]
    public abstract class MvxEventSourceActivity
        : Activity, IMvxEventSourceActivity
    {
        protected MvxEventSourceActivity()
        {
        }

        protected MvxEventSourceActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            CreateWillBeCalled?.Raise(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            CreateCalled?.Raise(this, savedInstanceState);
        }

        protected override void OnDestroy()
        {
            DestroyCalled?.Raise(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            NewIntentCalled?.Raise(this, intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCalled?.Raise(this);
        }

        protected override void OnPause()
        {
            PauseCalled?.Raise(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            StartCalled?.Raise(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            RestartCalled?.Raise(this);
        }

        protected override void OnStop()
        {
            StopCalled?.Raise(this);
            base.OnStop();
        }

        public override void StartActivityForResult(Intent? intent, int requestCode)
        {
            StartActivityForResultCalled?.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent? data)
        {
            ActivityResultCalled?.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled?.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled?.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler? DisposeCalled;

        public event EventHandler<CrossValueEventArgs<Bundle?>>? CreateWillBeCalled;

        public event EventHandler<CrossValueEventArgs<Bundle?>>? CreateCalled;

        public event EventHandler? DestroyCalled;

        public event EventHandler<CrossValueEventArgs<Intent?>>? NewIntentCalled;

        public event EventHandler? ResumeCalled;

        public event EventHandler? PauseCalled;

        public event EventHandler? StartCalled;

        public event EventHandler? RestartCalled;

        public event EventHandler? StopCalled;

        public event EventHandler<CrossValueEventArgs<Bundle>>? SaveInstanceStateCalled;

        public event EventHandler<CrossValueEventArgs<MvxStartActivityForResultParameters>>? StartActivityForResultCalled;

        public event EventHandler<CrossValueEventArgs<MvxActivityResultParameters>>? ActivityResultCalled;
    }
}
