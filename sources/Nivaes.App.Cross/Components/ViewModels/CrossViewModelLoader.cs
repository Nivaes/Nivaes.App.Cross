using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    internal class CrossViewModelLoader
        : ICrossViewModelLoader
    {

        private ICrossViewModelLocator _viewModelLocator;
        //protected ICrossViewModelLocatorCollection LocatorCollection { get; }

        //public CrossViewModelLoader(ICrossViewModelLocatorCollection locatorCollection)
        //{
        //    LocatorCollection = locatorCollection;
        //}

        public CrossViewModelLoader(ICrossViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;
        }

        // Reload should be used to re-run cached ViewModels lifecycle if required.
        public ICrossViewModel ReloadViewModel(ICrossViewModel viewModel, CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                viewModel = _viewModelLocator.Reload(viewModel, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new CrossException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
   
            }

            return viewModel;
        }

        public ICrossViewModel ReloadViewModel<TParameter>(ICrossViewModel<TParameter> viewModel, TParameter param, CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            //var viewModelLocator = FindViewModelLocator(request);

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Reload(viewModel, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new CrossException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        public ICrossViewModel LoadViewModel(CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        {
            //if (request.ViewModelType == typeof(CrossNullViewModel))
            //{
            //    return new CrossNullViewModel();
            //}
            if(request.ViewModelType == null)
                return null;

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Load(request.ViewModelType, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new CrossException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        //public ICrossViewModel LoadViewModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        //            CrossViewModelRequest request, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs = null)
        //        where TViewModel : ICrossViewModel
        //{
        //    //if (request.ViewModelType == typeof(CrossNullViewModel))
        //    //{
        //    //    return new CrossNullViewModel();
        //    //}
        //    if (request.ViewModelType == null)
        //        return null;

        //    var parameterValues = new CrossBundle(request.ParameterValues);
        //    try
        //    {
        //        return _viewModelLocator.Load<TViewModel>(parameterValues, savedState, navigationArgs);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new CrossException(ex,
        //            $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
        //    }
        //}

        public ICrossViewModel? LoadViewModel<TParameter>(CrossViewModelRequest request, TParameter param, ICrossBundle? savedState,
           ICrossNavigateEventArgs? navigationArgs = null)
        {
            //if (request.ViewModelType == null || request.ViewModelType == typeof(CrossNullViewModel))
            //{
            //    return new CrossNullViewModel();
            //}
            if (request.ViewModelType == null)
                return null;

            var parameterValues = new CrossBundle(request.ParameterValues);
            try
            {
                return _viewModelLocator.Load<TParameter>(request.ViewModelType!, param, parameterValues, savedState, navigationArgs);
            }
            catch (Exception ex)
            {
                throw new CrossException(ex, $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
            }
        }

        //public ICrossViewModel LoadViewModel<TParameter>(
        //    CrossViewModelRequest request, TParameter param, ICrossBundle? savedState, ICrossNavigateEventArgs? navigationArgs)
        //{
        //    if (request.ViewModelType == null || request.ViewModelType == typeof(CrossNullViewModel))
        //    {
        //        return new CrossNullViewModel();
        //    }

        //    var parameterValues = new CrossBundle(request.ParameterValues);
        //    try
        //    {
        //        return _viewModelLocator.Load(request.ViewModelType!, parameterValues, savedState, navigationArgs);
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception.Wrap(
        //            $"Failed to construct and initialize ViewModel for type {request.ViewModelType} from locator {_viewModelLocator.GetType().Name} - check InnerException for more information");
        //    }
        //}

        //private ICrossViewModelLocator FindViewModelLocator(CrossViewModelRequest request)
        //{
        //    var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

        //    if (viewModelLocator == null)
        //    {
        //        throw new CrossException($"Sorry - somehow there's no viewmodel locator registered for {request.ViewModelType}");
        //    }

        //    return viewModelLocator;
        //}
    }
}