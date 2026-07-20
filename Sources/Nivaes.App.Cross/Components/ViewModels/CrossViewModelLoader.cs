namespace Nivaes.App.Cross
{
    internal sealed class CrossViewModelLoader
        //: ICrossViewModelLoader
    {
        private CrossViewModelLocator _viewModelLocator;

        public CrossViewModelLoader(CrossViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                viewModel = _viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");

            }

            return viewModel;
        }

        public ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param, ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel LoadViewModel(Type viewModelType,
            IDictionary<string, string> parameterValues, 
            ICrossBundle? savedState, 
            ICrossNavigateEventArgs? navigationArgs = null)
        {
            var bundleParameterValues = new CrossBundle(parameterValues);
            try
            {
                return _viewModelLocator.Load(viewModelType, bundleParameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {viewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel? LoadViewModel<TParameter>(
                ViewModelRequest request,
                TParameter param, 
                ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
           where TParameter : notnull
        {
            //if (request.ViewModelType == null)
            //    return null; // ToDo: Puede haber un viewModelType == null?

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Load<TParameter>(request.ViewModelType, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }
    }
}