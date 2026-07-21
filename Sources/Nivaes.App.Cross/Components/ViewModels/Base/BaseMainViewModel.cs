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
        protected BaseMainViewModel(ILogger logger)
           : base(logger)
        { }
    }

    public abstract class BaseMainViewModel<TParameter>
        : BaseViewModel<TParameter>, IBaseMainViewModel
            where TParameter : class
    {
        protected BaseMainViewModel(ILogger logger)
           : base(logger)
        { }
    }

    public abstract class BaseMainViewModelResult<TResult>
     : BaseViewModelResult<TResult>, IBaseMainViewModel
    {
        protected BaseMainViewModelResult(ILogger logger)
           : base(logger)
        { }
    }

    public abstract class BaseMainViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IBaseMainViewModel
    {
        protected BaseMainViewModel(ILogger logger)
           : base(logger)
        { }
    }
}
