namespace Nivaes.App.Cross.UIKit.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;

    public abstract class RootViewPressentation : UIKitViewPressentation
    {
        protected RootViewPressentation(AppDataModel appDataModel)
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
