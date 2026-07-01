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
        protected BaseDetailViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class BaseDetailViewModel<TParameter>
        : BaseMainViewModel<TParameter>, IBaseDetailViewModel
            where TParameter : class
    {
        protected BaseDetailViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }

    //public abstract class BaseDetailViewModelResult<TResult>
    //    : BaseMainViewModelResult<TResult>, IBaseDetailViewModel
    //{
    //    protected BaseDetailViewModelResult(ILoggerFactory logFactory, IMvxNavigationService navigationService)
    //        : base(logFactory, navigationService)
    //    { }
    //}

    //public abstract class BaseDetailViewModel<TParameter, TResult>
    //    : BaseMainViewModel<TParameter, TResult>, IBaseDetailViewModel
    //{
    //    protected BaseDetailViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
    //        : base(logFactory, navigationService)
    //    { }
    //}
}
