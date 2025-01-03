namespace Nivaes.App.Cross.UIKit.Presenters
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;

    public abstract class UIKitViewPresentation : ViewPresentation
    {
        protected AppDataModel AppDataModel { get; private set; }

        protected UIKitViewPresentation(AppDataModel appDataModel)
        {
            this.AppDataModel = appDataModel;
        }

        //public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        //{
        //    var storyboard = UIStoryboard.FromName("RootView", null);
        //    var viewController = storyboard.InstantiateViewController(viewType.Name);
        //    //return (IMvxIosView)viewController;

        //    return Task.FromResult(false);
        //}

        public override Task<bool> CloseView(IViewModel request)
        {
            return Task.FromResult(false);
        }
    }
}
