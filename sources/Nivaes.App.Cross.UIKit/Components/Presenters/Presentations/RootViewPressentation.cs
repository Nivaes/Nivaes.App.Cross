namespace Nivaes.App.Cross.UIKit.Presenters
{
    using System;
    using System.Threading.Tasks;

    public class RootViewPressentation : UIKitViewPressentation
    {
        public RootViewPressentation(AppDataModel appDataModel)
            : base(appDataModel)
        {
        }

        public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        {
            try
            {
                //var UIKitRequest = (IUIKitViewModelRequest)request;
                //var storyboardName = UIKitRequest.StoryboardName ?? viewType.Name;
                var storyboardName = viewType.Name; 

                var storyboard = UIStoryboard.FromName(storyboardName, null);
                var viewController = storyboard.InstantiateViewController(viewType.Name);

                AppDataModel.Windows.RootViewController = viewController;
            }
            catch(Exception ex)
            {
                throw;
            }

            return Task.FromResult(true);
        }

        //public override Task<bool> ShowView(Type viewType, IViewModelRequest request)
        //{

        //    return Task.FromResult(false);
        //}

        //public override Task<bool> CloseView(IViewModel request)
        //{
        //    return Task.FromResult(false);
        //}
    }
}
