using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App
{
    public abstract class BaseDefaultDetailViewModel
        : BaseMainViewModel
    {
        protected BaseDefaultDetailViewModel(ILogger logger)
           : base(logger)
        { }

        public abstract string NoSelectedMessageText { get; }
    }
}
