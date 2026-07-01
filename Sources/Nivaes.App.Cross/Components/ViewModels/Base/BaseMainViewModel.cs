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
        protected BaseMainViewModel(ICrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }

    public abstract class BaseMainViewModel<TParameter>
        : BaseViewModel<TParameter>, IBaseMainViewModel
            where TParameter : class
    {
        protected BaseMainViewModel(ICrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
    }
}
