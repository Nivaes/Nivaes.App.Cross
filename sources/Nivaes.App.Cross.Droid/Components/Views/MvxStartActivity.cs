using Android.Runtime;
using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    [Register("mvvmcross.platforms.android.views.MvxStartActivity")]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public abstract class MvxStartActivity
        : MvxActivity
    {
        protected const int NoContent = 0;

        private readonly int _resourceId;

        private Bundle _bundle;

        public new CrossNullViewModel ViewModel
        {
            get { return base.ViewModel as CrossNullViewModel; }
            set { base.ViewModel = value; }
        }

        protected MvxStartActivity(int resourceId = NoContent)
        {
            RegisterSetup();
            _resourceId = resourceId;
        }

        protected MvxStartActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected virtual void RequestWindowFeatures()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeatures();

            _bundle = savedInstanceState;

            base.OnCreate(savedInstanceState);

            if (_resourceId != NoContent)
            {
                // Set our view from the "splash" layout resource
                // Be careful to use non-binding inflation
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }

#pragma warning disable AsyncFixer01, AsyncFixer03
        protected override async void OnResume()
        {
            base.OnResume();
            await RunAppStartAsync(_bundle);
        }
#pragma warning restore AsyncFixer01, AsyncFixer03

        protected virtual async Task RunAppStartAsync(Bundle bundle)
        {
            if (Mvx.IoCProvider?.TryResolve(out ICrossAppStart startup) == true)
            {
                if (!startup.IsStarted)
                {
                    await startup.StartAsync(GetAppStartHint(bundle));
                }
                else
                {
                    Finish();
                }
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual void RegisterSetup()
        {
        }
    }
}