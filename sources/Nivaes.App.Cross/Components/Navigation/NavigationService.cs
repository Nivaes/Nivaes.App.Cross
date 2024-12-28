namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.IoC;

    public class NavigationService : INavigationService
    {
        protected readonly IViewDispatcher mViewDispatcher;

        public NavigationService(IViewDispatcher viewDispatcher)
        {
            mViewDispatcher = viewDispatcher;
        }

        public Task<bool> Navigate<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : IViewModel
        {
            IViewModelRequest request = new ViewModelRequest<TViewModel>();

            return Navigate<TViewModel>(request, cancellationToken);
        }

        private async Task<bool> Navigate<TViewModel>(IViewModelRequest request, CancellationToken cancellationToken = default)
             where TViewModel : IViewModel
        {
            request.ViewModel = ViewModelLoader.LoadViewModel<TViewModel>();

            var hasNavigated  = await mViewDispatcher.ShowViewModel(request).ConfigureAwait(false);

            if (!hasNavigated)
                return false;

            return true;
        }
    }
}
