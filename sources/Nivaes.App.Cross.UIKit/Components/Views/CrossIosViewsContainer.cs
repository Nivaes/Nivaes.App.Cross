using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Nivaes.App.Cross.UIKit
{
    public class CrossIosViewsContainer
        : CrossViewsContainer
        , ICrossIosViewsContainer
    {
        public ICrossViewModelRequest? CurrentRequest { get; private set; }

        ICrossViewModelRequest ICrossCurrentRequest.CurrentRequest => throw new NotImplementedException();

        public virtual ICrossIosView CreateView(ICrossViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new CrossException("View Type not found for " + request.ViewModelType);

                var view = CreateViewOfType(viewType);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual ICrossIosView CreateView(ICrossViewModel viewModel)
        {
            throw new NotImplementedException();
            //var request = new CrossViewModelInstanceRequest(viewModel);
            //var view = CreateView(request);
            //return view;
        }

        public virtual ICrossIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
        {
            var storyboardAttribute = viewType.GetCustomAttribute<CrossFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = UIStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateViewController(viewType.Name);
                    return (ICrossIosView)viewController;
                }
                catch (Exception ex)
                {
                    throw new CrossException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            if (Activator.CreateInstance(viewType) is not ICrossIosView view)
                throw new CrossException("View not loaded for " + viewType);

            return view;
        }
    }
}
