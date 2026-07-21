using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IBaseDetailViewModel
         : IBaseViewModel
    {
    }

    public abstract class BaseDetailViewModel
        : BaseMainViewModel, IBaseDetailViewModel
    {
        protected BaseDetailViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class BaseDetailViewModel<TParameter>
        : BaseMainViewModel<TParameter>, IBaseDetailViewModel
            where TParameter : class
    {
        protected BaseDetailViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }

    public abstract class BaseDetailViewModelResult<TResult>
        : BaseMainViewModelResult<TResult>, IBaseDetailViewModel
    {
        protected BaseDetailViewModelResult(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }

    public abstract class BaseDetailViewModel<TParameter, TResult>
        : BaseMainViewModel<TParameter, TResult>, IBaseDetailViewModel
    {
        protected BaseDetailViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
