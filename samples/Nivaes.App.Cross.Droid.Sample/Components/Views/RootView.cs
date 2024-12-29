namespace Nivaes.App.Cross.Droid.Sample
{
    using Nivaes.App.Cross.Sample;

    [Activity(Label = "@string/app_name")]
    public class RootView : CrossActivity<RootViewModel>
    {
        public RootView()
            //: base(Resource.Layout.activity_root)
        {

        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_root);
        }
    }
}