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
            var UIKitRequest = (IUIKitViewModelRequest)request;
            var storyboardName = UIKitRequest.StoryboardName ?? viewType.Name;

            var storyboard = UIStoryboard.FromName(storyboardName, null);
            var viewController = storyboard.InstantiateViewController(viewType.Name);

            AppDataModel.Windows.RootViewController = viewController;

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
