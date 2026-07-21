using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IDialogViewModel
        : IBaseViewModel
    {
        #region Localization
        string AcceptButtonLabel { get; }
        string CancelButtonLabel { get; }
        #endregion

        ICrossAsyncCommand CloseCommand { get; }
    }

    public abstract class DialogViewModel
        : BaseViewModel, IDialogViewModel
    {
        #region Localization
        public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
        public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
        #endregion

        protected DialogViewModel(ILogger logger)
            : base(logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }

    public abstract class DialogViewModel<TParameter>
        : BaseViewModel<TParameter>, IDialogViewModel
    {
        #region Localization
        public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
        public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
        #endregion

        protected DialogViewModel(ILogger logger)
            : base(logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }

    public abstract class DialogViewModelResult<TResult>
       : BaseViewModelResult<TResult>, IDialogViewModel
    {
        #region Localization
        public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
        public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
        #endregion

        protected DialogViewModelResult(ILogger logger)
            : base(logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }

    public abstract class DialogViewModel<TParameter, TResult>
        : BaseViewModel<TParameter, TResult>, IDialogViewModel
    {
        #region Localization
        public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
        public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
        #endregion

        protected DialogViewModel(ILogger logger)
            : base(logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }
}
