using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class BaseMasterDetailViewModel
        : BaseMainViewModel
    {
        protected BaseMasterDetailViewModel(ILogger logger)
            : base(logger)
        { }
    }
}
