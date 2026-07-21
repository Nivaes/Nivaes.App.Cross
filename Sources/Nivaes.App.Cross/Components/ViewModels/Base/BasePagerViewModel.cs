using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IBasePagerViewModel
      : IBaseViewModel, ICrossViewModel
    {
    }

    public abstract class BasePagerViewModel
        : BaseViewModel, IBasePagerViewModel
    {
        protected BasePagerViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class BasePagerViewModel<TParameter>
       : BaseViewModel<TParameter>, IBasePagerViewModel
            where TParameter : class
    {
        protected BasePagerViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }
}
