using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
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
                var viewType = GetViewType(request.ViewModelType!);
                if (viewType == null)
                    throw new AppException($"View Type not found for {request.ViewModelType}");

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
                    throw new AppException(ex, $"Loading view of type {viewType.Name} from storyboard '{storyboardName}' failed: {2}");
                }
            }

            if (ActivatorUtilities.CreateInstance(_serviceProvider, viewType) is not IMvxIosView view)
                throw new AppException($"View not loaded for {viewType}");

            return view;
        }
    }
}
