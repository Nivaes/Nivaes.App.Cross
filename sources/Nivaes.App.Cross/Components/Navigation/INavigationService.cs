namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;

    public interface INavigationService
    {
        public delegate void BeforeNavigateEventHandler(object sender, INavigateEventArgs e);

        public delegate void AfterNavigateEventHandler(object sender, INavigateEventArgs e);

        public delegate void BeforeCloseEventHandler(object sender, INavigateEventArgs e);

        public delegate void AfterCloseEventHandler(object sender, INavigateEventArgs e);

        public delegate void BeforeChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

        public delegate void AfterChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);


        Task<bool> Navigate<TViewModel>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : IViewModel;

        Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
           where TViewModel : IViewModel<TParameter>;

        Task<TResult?> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) 
            where TViewModel : IViewModelResult<TResult>;

        Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter param, ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
            where TViewModel : IViewModel<TParameter, TResult>;

        Task<bool> CanNavigate<TViewModel>() 
            where TViewModel : IViewModel;

        Task<bool> Close(IViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> Close<TResult>(IViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default(CancellationToken));
    }
}
