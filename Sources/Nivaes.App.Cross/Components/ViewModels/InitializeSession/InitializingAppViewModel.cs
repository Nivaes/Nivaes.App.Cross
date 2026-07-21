using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class InitializingAppViewModel
        : BaseViewModel
    {
        #region Properties
        private readonly IIdentifyService mIdentifyService;
        private readonly IMediaService mMediaService;
        #endregion

        #region Life cycle
        public InitializingAppViewModel(
                IIdentifyService identifyService, 
                IMediaService mediaService,
                ILogger<InitializingAppViewModel> logger)
            : base(logger)
        {
            mIdentifyService = identifyService;
            mMediaService = mediaService;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            Task.Run(async () =>
            {
                await mIdentifyService.InitializingApp().ConfigureAwait(false);

                await mMediaService.Initialize().ConfigureAwait(false);
            });
        }
        #endregion
    }
}
