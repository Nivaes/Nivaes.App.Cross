namespace Nivaes.App.Cross.AppKitOS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class MvxMacViewsContainer
        : CrossViewsContainer, IMvxMacViewsContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public CrossViewModelRequest? CurrentRequest { get; private set; }

        public MvxMacViewsContainer(IServiceProvider serviceProvider, ILogger<MvxMacViewsContainer> logger)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual IMvxMacView CreateView(CrossViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType!);
                if (viewType == null)
                    throw new CrossException("View Type not found for " + request.ViewModelType);

                var view = CreateViewOfType(viewType, request);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual IMvxMacView CreateViewOfType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type viewType, CrossViewModelRequest request)
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
                    throw new CrossException("Loading view of type {0} from storyboard {1} failed: {2}", viewType.Name, storyboardName, ex.Message);
                }
            }

            var view = ActivatorUtilities.CreateInstance(_serviceProvider, viewType) as IMvxMacView;
            if (view == null)
                throw new CrossException("View not loaded for " + viewType);
            return view;
        }

        public virtual IMvxMacView CreateView(ICrossViewModel viewModel)
        {
            var request = new CrossViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }
    }
}
