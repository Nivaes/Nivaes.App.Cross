namespace Nivaes.App.Cross.Droid.Presenters
{
    using System.Reflection.Metadata;
    using Android.Content;
    using Android.OS;
    using Nivaes.App.Cross.Presenters;

    public abstract class DroidViewPresentation : ViewPresentation
    {
        protected AppDataModel AppDataModel { get; private set; }

        protected DroidViewPresentation(AppDataModel appDataModel)
        {
            this.AppDataModel = appDataModel;
        }

        public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        {
            var intent = new Intent(AppDataModel.ApplicationContext, viewType);
            //intent.PutExtra("request", request);
            //var activity = CurrentActivity;
            //Bundle? bundle = new Bundle();
            //Context context;

            //AppDataModel.ApplicationContext.StartActivity(intent, bundle);
            AppDataModel.ApplicationContext.StartActivity(intent);

            return Task.FromResult(false);
        }

        public override Task<bool> CloseView(IViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
