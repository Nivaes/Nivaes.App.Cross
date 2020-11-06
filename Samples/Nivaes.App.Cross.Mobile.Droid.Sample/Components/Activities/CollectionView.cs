namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using Android.App;
    using Android.OS;
    using MvvmCross.Platforms.Android.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Mobile.Sample;

    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    public class CollectionView
        : MvxActivity<CollectionViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CollectionView);
        }
    }
}
