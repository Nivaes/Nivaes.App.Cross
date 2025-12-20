namespace MvvmCross.ViewModels
{
    using MvvmCross.Exceptions;
    using MvvmCross.Navigation.EventArguments;
    using Nivaes.App.Cross;

    public class MvxViewModelLoader
        : IMvxViewModelLoader
    {
        protected IMvxViewModelLocatorCollection LocatorCollection { get; }

        public MvxViewModelLoader(IMvxViewModelLocatorCollection locatorCollection)
        {
            LocatorCollection = locatorCollection;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, MvxViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null)
        {
            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                viewModel = viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information");
            }

            return viewModel;
        }

        public ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param, MvxViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null)
        {
            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel LoadViewModel(MvxViewModelRequest request, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return viewModelLocator.Load(request.ViewModelType!, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel LoadViewModel<TParameter>(
            MvxViewModelRequest request, TParameter param, ICrossBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null)
        {
            if (request.ViewModelType == typeof(MvxNullViewModel))
            {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return viewModelLocator.Load(request.ViewModelType!, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null)
            {
                throw new MvxException($"Sorry - somehow there's no viewmodel locator registered for {request.ViewModelType}");
            }

            return viewModelLocator;
        }
    }
}