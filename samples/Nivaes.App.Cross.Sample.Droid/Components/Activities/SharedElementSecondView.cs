namespace Playground.Droid.Activities
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Platforms.Android.Presenters.Attributes;
    using MvvmCross.Platforms.Android.Views;
    using Nivaes.App.Cross.Droid;
    using Nivaes.App.Cross.Sample.Droid;
    using Playground.Core.ViewModels;
    using Resource = Nivaes.App.Cross.Sample.Droid.Resource;

    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    [RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
    public sealed class SharedElementSecondView 
        : MvxActivity<SharedElementSecondViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SharedElementSecondView);

            Bundle extras = Intent.Extras;
            extras.SetSharedElementsById(FindViewById(Android.Resource.Id.Content));
        }
    }
}
