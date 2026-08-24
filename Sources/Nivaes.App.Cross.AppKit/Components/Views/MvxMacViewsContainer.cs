using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    internal class MvxMacViewsContainer
        : IMvxMacViewsContainer
    {
        public IViewModelRequest? CurrentRequest { get; private set; }
        private readonly ILogger _logger;

        public MvxMacViewsContainer(ILogger<MvxMacViewsContainer> logger)
        {
            _logger = logger;
        }

        public virtual IMvxMacView CreateView(IViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = Singleton<ViewsContainers>.Instance.ViewModelViews[request.ViewModelType];

                var view = CreateViewOfType(viewType, request);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual IMvxMacView CreateViewOfType(Type viewType, IViewModelRequest request)
        {
            var storyboardAttribute = viewType.GetCustomAttribute<MvxFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = NSStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateControllerWithIdentifier(viewType.Name);
                    return (IMvxMacView)viewController;
                }
                catch (Exception ex)
                {
                    throw new AppException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            var aa = viewType.FullName;
            var view = Activator.CreateInstance(viewType) as IMvxMacView;
            if (view == null)
                throw new AppException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxMacView CreateView(ICrossViewModel viewModel)
        {
            var request = new ViewModelRequest(viewModel);
            var view = CreateView(request);
            return view;
        }
    }
}
