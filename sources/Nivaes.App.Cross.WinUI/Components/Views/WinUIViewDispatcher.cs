namespace Nivaes.App.Cross.WinUI
{
    using Nivaes.App.Cross.Presenters;

    public class WinUIViewDispatcher : IViewDispatcher
    {
        private readonly IViewPresenter mViewPresenter;

        public WinUIViewDispatcher(IViewPresenter viewPresenter)
        {
            mViewPresenter = viewPresenter;
        }

        public async Task<bool> ShowViewModel(IViewModelRequest request)
        {
            await mViewPresenter.Show(request);

            return true;
        }
    }
}
