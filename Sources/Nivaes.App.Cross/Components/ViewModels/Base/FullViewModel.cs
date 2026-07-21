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
        protected FullViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModel<TParameter>
        : BaseViewModel<TParameter>, IFullViewModel
            where TParameter : class
    {
        protected FullViewModel(CrossNavigationService navigationService, ILogger logger)
             : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModelResult<TResult>
       : BaseViewModelResult<TResult>, IFullViewModel
    {
        protected FullViewModelResult(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class FullViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IFullViewModel
    {
        protected FullViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }
}
