using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS
{
    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;
        

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter, ILogger<MvxMacViewDispatcher> logger)
            :base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Func<Task> action = () =>
            {
                Logger.LogTrace($"Navigate requested{request.ViewModelType!.FullName}");
                return _presenter.Show(request);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            Func<Task> action = () =>
            {
                Logger.LogTrace($"Change presentation requested");
                return _presenter.ChangePresentation(hint);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }
    }
}
