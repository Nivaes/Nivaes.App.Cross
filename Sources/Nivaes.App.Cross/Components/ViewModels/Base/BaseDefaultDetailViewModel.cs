namespace Nivaes.App
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public abstract class BaseDefaultDetailViewModel
        : BaseMainViewModel
    {
        protected BaseDefaultDetailViewModel(ILogger logger, ICrossNavigationService navigationService)
           : base(navigationService, logger)
        { }

        public abstract string NoSelectedMessageText { get; }
    }
}
