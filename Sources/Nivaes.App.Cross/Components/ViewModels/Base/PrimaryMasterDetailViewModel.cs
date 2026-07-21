using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class PrimaryMasterDetailViewModel
        : BaseMasterDetailViewModel
    {
        public PrimaryMasterDetailViewModel(ILogger logger)
            : base(logger)
        { }
    }
}
