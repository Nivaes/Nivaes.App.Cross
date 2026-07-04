using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IFullViewModel
        : IBaseViewModel
    {
    }

    public abstract class FullViewModel
        : BaseViewModel, IFullViewModel
    {
        protected FullViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModel<TParameter>
        : BaseViewModel<TParameter>, IFullViewModel
            where TParameter : class
    {
        protected FullViewModel(ICrossNavigationService navigationService, ILogger logger)
             : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModelResult<TResult>
       : BaseViewModelResult<TResult>, IFullViewModel
    {
        protected FullViewModelResult(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IFullViewModel
    {
        protected FullViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }
}
