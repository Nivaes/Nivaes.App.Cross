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

        protected DialogViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }

    public abstract class DialogViewModel<TParameter>
        : BaseViewModel<TParameter>, IDialogViewModel
            where TParameter : class
    {
        #region Localization
        public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
        public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
        #endregion

        protected DialogViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () =>
        {
            await base.NavigationService.Close(this).ConfigureAwait(false);
        });
    }

    //public abstract class DialogViewModelResult<TResult>
    //   : BaseViewModelResult<TResult>, IDialogViewModel
    //{
    //    #region Localization
    //    public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
    //    public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
    //    #endregion

    //    protected DialogViewModelResult(ILoggerFactory logFactory, IMvxNavigationService navigationService)
    //        : base(logFactory, navigationService)
    //    { }

    //    public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () =>
    //    {
    //        await base.NavigationService.Close(this).ConfigureAwait(false);
    //    });
    //}

    //public abstract class DialogViewModel<TParameter, TResult>
    //    : BaseViewModel<TParameter, TResult>, IDialogViewModel
    //{
    //    #region Localization
    //    public virtual string AcceptButtonLabel => DialogLocalizationString.AcceptButtonLabel;
    //    public virtual string CancelButtonLabel => DialogLocalizationString.CancelButtonLabel;
    //    #endregion

    //    protected DialogViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService)
    //        : base(logFactory, navigationService)
    //    { }

    //    public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () =>
    //    {
    //        await base.NavigationService.Close(this).ConfigureAwait(false);
    //    });
    //}
}
