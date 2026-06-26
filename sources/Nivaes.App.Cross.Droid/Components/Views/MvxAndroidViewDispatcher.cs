using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.Droid
{
    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(IAndroidViewPresenter presenter, ILogger<MvxAndroidViewDispatcher> logger)
            :base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
