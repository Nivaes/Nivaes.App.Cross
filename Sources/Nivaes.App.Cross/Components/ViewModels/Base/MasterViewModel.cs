using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IMasterViewModel
        : IBaseViewModel
    {
    }

    public abstract class MasterViewModel
        : BaseViewModel, IMasterViewModel
    {
        #region Constructor
        protected MasterViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
        #endregion

        #region ShowDefaultView
        private bool mIsShowDefaultDetail;

        protected async Task ShowDefaultDetailViewModel<TDefaultViewModel>()
            where TDefaultViewModel : BaseDefaultDetailViewModel
        {
            if (!mIsShowDefaultDetail)
            {
                mIsShowDefaultDetail = true;
                await base.NavigationService.Navigate<TDefaultViewModel>().ConfigureAwait(false);
            }
        }
        #endregion
    }

    public abstract class MasterViewModel<TParameter>
        : BaseViewModel<TParameter>, IMasterViewModel
            where TParameter : class
    {
        #region Constructor
        protected MasterViewModel(CrossNavigationService navigationService, ILogger logger)
           : base(navigationService, logger)
        { }
        #endregion

        #region ShowDefaultView
        private bool mIsShowDefaultDetail;

        protected async Task ShowDefaultDetailViewModel<TDefaultViewModel>()
            where TDefaultViewModel : BaseDefaultDetailViewModel
        {
            if (!mIsShowDefaultDetail)
            {
                mIsShowDefaultDetail = true;
                await base.NavigationService.Navigate<TDefaultViewModel>().ConfigureAwait(false);
            }
        }
        #endregion
    }
}
