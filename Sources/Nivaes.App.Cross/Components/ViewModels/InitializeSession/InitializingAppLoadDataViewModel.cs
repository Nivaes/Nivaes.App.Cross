using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class InitializingAppLoadDataViewModel
        : BaseViewModel
    {
        #region Properties
        private readonly IIdentifyService mIdentifyService;
        //private readonly ISyncronizationService mSyncronizationService;
        #endregion

        #region Life cycle
        public InitializingAppLoadDataViewModel(
                    IIdentifyService identifyService,
                    ILogger logger)
            : base(logger)
        {
            mIdentifyService = identifyService;
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();

            await mIdentifyService.InitializeApp().ConfigureAwait(false);
        }
        #endregion
    }
}
