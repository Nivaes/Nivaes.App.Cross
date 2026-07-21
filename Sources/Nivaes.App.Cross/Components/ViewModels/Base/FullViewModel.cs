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
        protected FullViewModel(ILogger logger)
          : base(logger)
        { }
    }

    public abstract class FullViewModel<TParameter>
        : BaseViewModel<TParameter>, IFullViewModel
            where TParameter : class
    {
        protected FullViewModel(ILogger logger)
             : base(logger)
        { }
    }

    public abstract class FullViewModelResult<TResult>
       : BaseViewModelResult<TResult>, IFullViewModel
    {
        protected FullViewModelResult(ILogger logger)
          : base(logger)
        { }
    }

    public abstract class FullViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IFullViewModel
    {
        protected FullViewModel(ILogger logger)
          : base(logger)
        { }
    }
}
