namespace Nivaes.App.Cross.Droid.Presenters
{
    using System.Reflection.Metadata;
    using Android.Content;
    using Android.OS;
    using Nivaes.App.Cross.Presenters;

    public abstract class DroidViewPresentation : 
        ViewPresentation
    {
        protected AppDataModel AppDataModel { get; private set; }

        protected DroidViewPresentation(AppDataModel appDataModel)
        {
            this.AppDataModel = appDataModel;
        }

        public override Task<bool> ShowView(Type viewType, ICrossViewModelRequest request)
        {
            var intent = new Intent(AppDataModel.ApplicationContext, viewType);
            //intent.PutExtra("request", request);
            //var activity = CurrentActivity;

            var viewModelKey = Singleton<TemporaryStore<ICrossViewModel>>.Instance.Add(request.ViewModel);
            Bundle bundle = new Bundle();
            bundle.PutInt("viewModelKey", viewModelKey);
            intent.PutExtras(bundle);

            AppDataModel.ApplicationContext.StartActivity(intent);

            return Task.FromResult(false);
        }

        public override Task<bool> CloseView(ICrossViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
