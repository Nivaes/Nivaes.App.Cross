namespace Nivaes.App.Cross.Droid
{
    using System.Threading.Tasks;
    using Android.OS;
    using Android.Views;
    using Nivaes.IoC;

    public abstract class CrossStartActivity : Activity
    {
        protected const int NoContent = 0;

        private readonly int _resourceId;
        private Bundle? _bundle;

        protected CrossStartActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.RequestWindowFeature(WindowFeatures.NoTitle);

            _bundle = savedInstanceState;

            base.OnCreate(savedInstanceState);

            if (_resourceId != NoContent)
            {
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }

        protected override async void OnResume()
        {
            base.OnResume();

            await RunAppStart(_bundle);
        }

        protected async Task RunAppStart(Bundle? bundle)
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddInstance(new AppDataModel(this));

            var application = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ICrossApplication>();

            if (application != null)
            {
                if (!application.ApplicationStart.IsStarted)
                {
                    await application.ApplicationStart.NavigateToFirstViewModel();
                }
                else
                {
                    base.Finish();
                }
            }
        }
    }
}
