using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class InitializingAppLoadDataViewModel
        : BaseViewModel
    {
        #region Properties
        private readonly IIdentifyService mIdentifyService;
        private readonly ISyncronizationService mSyncronizationService;
        #endregion

        #region Life cycle
        public InitializingAppLoadDataViewModel(IIdentifyService identifyService,
                    ISyncronizationService syncronizationService,
                    ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        {
            mSyncronizationService = syncronizationService;
            mIdentifyService = identifyService;
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();

            await mSyncronizationService.Initialize().ConfigureAwait(false);

            await mIdentifyService.InitializeApp().ConfigureAwait(false);
        }
        #endregion
    }
}
