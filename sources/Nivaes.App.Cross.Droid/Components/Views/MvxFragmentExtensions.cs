namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross;

    public static class MvxFragmentExtensions
    {
        extension(IMvxFragmentView fragmentView)
        {
            [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
            public Type FindAssociatedViewModelType(Type fragmentActivityParentType)
            {
                var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

                var type = fragmentView.GetType();

                if (viewModelType == null)
                {
                    if (!type.HasBasePresentationAttribute())
                        throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not have {nameof(MvxFragmentPresentationAttribute)} attribute set!");

                    var cacheableFragmentAttribute = type.GetBasePresentationAttribute();
                    if (cacheableFragmentAttribute.ViewModelType == null)
                        throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not use {nameof(MvxFragmentPresentationAttribute)} with ViewModel Type constructor.");

                    viewModelType = cacheableFragmentAttribute.ViewModelType;
                }

                return viewModelType;
            }

            public ICrossViewModel LoadViewModel(ICrossBundle savedState, Type fragmentParentActivityType, CrossViewModelRequest? request = null)
            {
                var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
                if (viewModelType == typeof(CrossNullViewModel))
                    return new CrossNullViewModel();

                if (viewModelType == null
                    || viewModelType == typeof(ICrossViewModel))
                {
                    CrossLogHost.Default?.Log(LogLevel.Trace,
                        "No ViewModel class specified for {FragmentViewType} in LoadViewModel",
                        fragmentView.GetType().Name);
                }

                if (request == null)
                    request = CrossViewModelRequest.GetDefaultRequest(viewModelType);

                var viewModelCache = Mvx.IoCProvider.Resolve<ICrossChildViewModelCache>();
                if (viewModelCache.Exists(viewModelType))
                {
                    var viewModelCached = viewModelCache.Get(viewModelType);
                    viewModelCache.Remove(viewModelType);
                    return viewModelCached;
                }

                var loaderService = Mvx.IoCProvider.Resolve<ICrossViewModelLoader>();
                var viewModel = loaderService.LoadViewModel(request, savedState);

                return viewModel;
            }
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure.")]
        public static void RunViewModelLifecycle(ICrossViewModel viewModel, ICrossBundle savedState,
            CrossViewModelRequest request)
        {
            try
            {
                if (request != null)
                {
                    var parameterValues = new CrossBundle(request.ParameterValues);
                    viewModel.CallBundleMethods("Init", parameterValues);
                }
                if (savedState != null)
                {
                    viewModel.CallBundleMethods("ReloadState", savedState);
                }
                viewModel.Start();
            }
            catch (Exception exception)
            {
                throw exception.Wrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            }
        }

        extension(Type fragmentType)
        {
            public string FragmentJavaName()
            {
                return Java.Lang.Class.FromType(fragmentType).Name;
            }
        }
    }
}
