using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class SecondaryMasterDetailViewModel
        : BaseMasterDetailViewModel
    {
        public SecondaryMasterDetailViewModel(ILogger logger)
            : base(logger)
        { }
    }
}
