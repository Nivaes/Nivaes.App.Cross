namespace Nivaes.App.Cross.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class MvxIosViewsContainer
        : CrossViewsContainer
        , IMvxIosViewsContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public CrossViewModelRequest? CurrentRequest { get; private set; }

        public MvxIosViewsContainer(IServiceProvider serviceProvider, ILogger<MvxIosViewsContainer> logger)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual IMvxIosView CreateView(CrossViewModelRequest request)
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

        public virtual IMvxIosView CreateView(ICrossViewModel viewModel)
        {
            var request = new CrossViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }

        public virtual IMvxIosView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType)
        {
            var storyboardAttribute = viewType.GetCustomAttribute<MvxFromStoryboardAttribute>();
            if (storyboardAttribute != null)
            {
                var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
                try
                {
                    var storyboard = UIStoryboard.FromName(storyboardName, null);
                    var viewController = storyboard.InstantiateViewController(viewType.Name);
                    return (IMvxIosView)viewController;
                }
                catch (Exception ex)
                {
                    throw new CrossException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            if (ActivatorUtilities.CreateInstance(_serviceProvider, viewType) is not IMvxIosView view)
                throw new CrossException("View not loaded for " + viewType);

            return view;
        }
    }
}
