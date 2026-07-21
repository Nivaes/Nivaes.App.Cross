using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class BaseMenuViewModel
        : BaseViewModel
    {
        #region Life cycle
        public BaseMenuViewModel(ILogger logger)
          : base(logger)
        { }
        #endregion
    }
}
