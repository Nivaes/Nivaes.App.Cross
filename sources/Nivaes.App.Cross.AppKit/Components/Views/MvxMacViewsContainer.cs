namespace MvvmCross.Platforms.Mac.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using AppKit;
    using MvvmCross.Exceptions;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxMacViewsContainer
        : CrossViewsContainer, IMvxMacViewsContainer
    {
        public CrossViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxMacView CreateView(CrossViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;
                var viewType = GetViewType(request.ViewModelType);
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

            var view = Activator.CreateInstance(viewType) as IMvxMacView;
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
