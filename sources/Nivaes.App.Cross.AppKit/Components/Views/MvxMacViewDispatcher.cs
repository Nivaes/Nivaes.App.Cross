using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitOS
{
    public class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IMvxMacViewPresenter _presenter;
        private readonly ILogger _logger;

        public MvxMacViewDispatcher(IMvxMacViewPresenter presenter, ILogger<MvxMacViewDispatcher> logger)
        {
            _presenter = presenter;
            _logger = logger;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Func<Task> action = () =>
            {
                _logger.LogTrace($"Navigate requested{request.ViewModelType!.FullName}");
                return _presenter.Show(request);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            Func<Task> action = () =>
            {
                _logger.LogTrace($"Change presentation requested");
                return _presenter.ChangePresentation(hint);
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }
    }
}
