namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using Android.App;
    using Android.OS;
    using MvvmCross.Platforms.Android.Views;
    using Nivaes.App.Cross.Presenters;

    [MvxActivityPresentation]
    [Activity(Label = "View for CustomBindingViewModel")]
    public class CustomBindingView : MvxActivity
    {
        protected override void OnCreate(Bundle? bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CustomBindingView);
        }
    }
}
