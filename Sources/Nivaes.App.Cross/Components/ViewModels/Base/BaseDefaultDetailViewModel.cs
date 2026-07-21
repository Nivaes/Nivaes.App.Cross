namespace Nivaes.App
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public abstract class BaseDefaultDetailViewModel
        : BaseMainViewModel
    {
        protected BaseDefaultDetailViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }

        public abstract string NoSelectedMessageText { get; }
    }
}
