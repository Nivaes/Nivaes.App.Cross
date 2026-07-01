using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class BaseMenuViewModel
        : BaseViewModel
    {
        #region Life cycle
        public BaseMenuViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
        #endregion
    }
}
