using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class SecondaryViewModel
        : BaseMainViewModel
    {
        public SecondaryViewModel(ILogger logger)
          : base(logger)
        { }
    }
}
