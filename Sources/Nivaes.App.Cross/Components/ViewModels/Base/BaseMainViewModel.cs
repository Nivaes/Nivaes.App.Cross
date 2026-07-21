using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IBaseMainViewModel
        : ICrossViewModel
    {
    }

    public abstract class BaseMainViewModel
        : BaseViewModel, IBaseMainViewModel
    {
        protected BaseMainViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }

    public abstract class BaseMainViewModel<TParameter>
        : BaseViewModel<TParameter>, IBaseMainViewModel
            where TParameter : class
    {
        protected BaseMainViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }

    public abstract class BaseMainViewModelResult<TResult>
     : BaseViewModelResult<TResult>, IBaseMainViewModel
    {
        protected BaseMainViewModelResult(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }

    public abstract class BaseMainViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IBaseMainViewModel
    {
        protected BaseMainViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }
}
