namespace Nivaes.App.Mobile.Droid.Sample
{
    using System;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Runtime;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using MvvmCross.Platforms.Android.Views;
    using Nivaes.App.Mobile.Sample;

    [Activity()]
    public class MainApplication
        : MvxAndroidApplication<Setup, AppMobileSampleApplication<AppMobileSampleAppStart>>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            AppCenter.Start("47506850-4340-4d2f-8610-eacfc4e0e956",
                    typeof(Analytics), typeof(Crashes));

            base.OnCreate();

            Xamarin.Essentials.Platform.Init(this);
        }

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    AppCenter.Start("47506850-4340-4d2f-8610-eacfc4e0e956",
        //        typeof(Analytics), typeof(Crashes));

        //    base.OnCreate(savedInstanceState);

        //    Xamarin.Essentials.Platform.Init(this, savedInstanceState);

        //    // Set our view from the "main" layout resource
        //    SetContentView(Resource.Layout.activity_main);
        //}
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
    }
}
