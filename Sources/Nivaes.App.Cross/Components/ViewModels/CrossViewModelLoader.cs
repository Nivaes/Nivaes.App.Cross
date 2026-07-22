namespace Nivaes.App.Cross
{
    public sealed class CrossViewModelLoader
    {
        private CrossViewModelLocator _viewModelLocator;

        public CrossViewModelLoader(CrossViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;
        }

        public ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, ViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");

            }
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

        public ICrossViewModel LoadViewModel(
            IViewModelRequest request,
            ICrossBundle? savedState,
            ICrossNavigateEventArgs? navigationArgs = null)
        {
            return LoadViewModel(request.ViewModelType, request.ParameterValues, savedState, navigationArgs);
        }

        public ICrossViewModel LoadViewModel(
            Type viewModelType,
            IDictionary<string, string>? parameterValues,
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
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {viewModelType.FullName} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public TViewModel LoadViewModel<TViewModel>(
            IDictionary<string, string>? parameterValues,
            ICrossBundle? savedState,
            ICrossNavigateEventArgs? navigationArgs = null)
            where TViewModel : ICrossViewModel
        {
            var bundleParameterValues = new CrossBundle(parameterValues);
            try
            {
                return _viewModelLocator.Load<TViewModel>(bundleParameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {typeof(TViewModel).FullName} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel LoadViewModel<TParameter>(
                Type viewModelType,
                IDictionary<string, string>? parameterValues,
                TParameter param, 
                ICrossBundle? savedState,
                ICrossNavigateEventArgs? navigationArgs = null)
           where TParameter : notnull
        {
            var bundleParameterValues = new CrossBundle(parameterValues);
            try
            {
                return _viewModelLocator.Load<TParameter>(viewModelType, param, bundleParameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {viewModelType.FullName} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public TViewModel LoadViewModel<TViewModel, TParameter>(
                IDictionary<string, string>? parameterValues,
                TParameter param,
                ICrossBundle? savedState,
                ICrossNavigateEventArgs? navigationArgs = null)
            where TViewModel : ICrossViewModel<TParameter>
            where TParameter : notnull
        {
            var bundleParameterValues = new CrossBundle(parameterValues);
            try
            {
                return _viewModelLocator.Load<TViewModel, TParameter>(param, bundleParameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Failed to construct and initialize ViewModel for type {typeof(TViewModel).FullName} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }
    }
}