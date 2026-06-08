namespace Nivaes.App.Cross.UIKitOS
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MvxIosViewDispatcher
        : MvxIosUIThreadDispatcher, ICrossViewDispatcher
    {
        private readonly IMvxIosViewPresenter _presenter;

        public MvxIosViewDispatcher(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(CrossViewModelRequest request)
        {
            Task action()
            {
                var logger = IPlatformApplication.Current?.Services.GetRequiredService<ILogger<MvxIosViewDispatcher>>();
                logger?.LogTrace("Navigate requested to {ViewModelType}", request?.ViewModelType);

                return _presenter.Show(request);
            }
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
            return true;
        }
    }
}
