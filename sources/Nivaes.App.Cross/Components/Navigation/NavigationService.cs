namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NavigationService : INavigationService
    {
        public IViewDispatcher ViewDispatcher { get; }

        public NavigationService(IViewDispatcher viewDispatcher)
        {
            ViewDispatcher = viewDispatcher;
        }

        public Task<bool> Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel
        {
            IViewModelRequest request = new ViewModelRequest<TViewModel>();

            return Navigate(request, cancellationToken);
        }

        private async Task<bool> Navigate(IViewModelRequest request, CancellationToken cancellationToken = default)
        {
            var hasNavigated  = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(false);

            if (!hasNavigated)
                return false;

            return true;
        }
    }
}
