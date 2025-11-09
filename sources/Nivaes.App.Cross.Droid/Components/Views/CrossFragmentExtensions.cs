namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Platforms.Android.Presenters.Attributes;

    public static class CrossFragmentExtensions
    {
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Fragment types are preserved by the Android presenter infrastructure and their associated attributes.")]
        [UnconditionalSuppressMessage("Trimming", "IL2073", Justification = "ViewModel types from FindAssociatedViewModelTypeOrNull and presentation attributes are preserved by the navigation infrastructure.")]
        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public static Type FindAssociatedViewModelType(this ICrossFragmentView fragmentView, Type fragmentActivityParentType)
        {
            throw new NotImplementedException();
            //var viewModelType = fragmentView.FindAssociatedViewModelTypeOrNull();

            //var type = fragmentView.GetType();

            //if (viewModelType == null)
            //{
            //    if (!type.HasBasePresentationAttribute())
            //        throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not have {nameof(MvxFragmentPresentationAttribute)} attribute set!");

            //    var cacheableFragmentAttribute = type.GetBasePresentationAttribute();
            //    if (cacheableFragmentAttribute.ViewModelType == null)
            //        throw new InvalidOperationException($"Your fragment of type {type.FullName} is not generic and it does not use {nameof(MvxFragmentPresentationAttribute)} with ViewModel Type constructor.");

            //    viewModelType = cacheableFragmentAttribute.ViewModelType;
            //}

            //return viewModelType;
        }

        public static ICrossViewModel LoadViewModel(this ICrossFragmentView fragmentView, ICrossBundle savedState, Type fragmentParentActivityType,
            ICrossViewModelRequest request = null)
        {
            throw new NotImplementedException();
            //var viewModelType = fragmentView.FindAssociatedViewModelType(fragmentParentActivityType);
            //if (viewModelType == typeof(CrossNullViewModel))
            //    return new CrossNullViewModel();

            //if (viewModelType == null
            //    || viewModelType == typeof(ICrossViewModel))
            //{
            //    CrossLogHost.Default?.Log(LogLevel.Trace, "No ViewModel class specified for {fragmentViewType} in LoadViewModel",
            //        fragmentView.GetType().Name);
            //}

            //if (request == null)
            //    request = CrossViewModelRequest.GetDefaultRequest(viewModelType);

            //var viewModelCache = Mvx.IoCProvider.Resolve<ICrossChildViewModelCache>();
            //if (viewModelCache.Exists(viewModelType))
            //{
            //    var viewModelCached = viewModelCache.Get(viewModelType);
            //    viewModelCache.Remove(viewModelType);
            //    return viewModelCached;
            //}

            //var loaderService = Mvx.IoCProvider.Resolve<ICrossViewModelLoader>();
            //var viewModel = loaderService.LoadViewModel(request, savedState);

            //return viewModel;
        }

        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ViewModel types are preserved by the navigation infrastructure.")]
        public static void RunViewModelLifecycle(ICrossViewModel viewModel, ICrossBundle savedState,
            ICrossViewModelRequest request)
        {
            throw new NotImplementedException();
            //try
            //{
            //    if (request != null)
            //    {
            //        var parameterValues = new CrossBundle(request.ParameterValues);
            //        viewModel.CallBundleMethods("Init", parameterValues);
            //    }
            //    if (savedState != null)
            //    {
            //        viewModel.CallBundleMethods("ReloadState", savedState);
            //    }
            //    viewModel.Start();
            //}
            //catch (Exception exception)
            //{
            //    throw exception.Wrap("Problem running viewModel lifecycle of type {0}", viewModel.GetType().Name);
            //}
        }

        public static string FragmentJavaName(this Type fragmentType)
        {
            return Java.Lang.Class.FromType(fragmentType).Name;
        }
    }
}
