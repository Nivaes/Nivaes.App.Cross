namespace Nivaes.App.Cross
{
    using System.Threading;
    using System.Threading.Tasks;

    public delegate void BeforeNavigateEventHandler(object sender, ICrossNavigateEventArgs e);

    public delegate void AfterNavigateEventHandler(object sender, ICrossNavigateEventArgs e);

    public delegate void BeforeCloseEventHandler(object sender, ICrossNavigateEventArgs e);

    public delegate void AfterCloseEventHandler(object sender, ICrossNavigateEventArgs e);

    public delegate void BeforeChangePresentationEventHandler(object sender, CrossChangePresentationEventArgs e);

    public delegate void AfterChangePresentationEventHandler(object sender, CrossChangePresentationEventArgs e);

    public interface ICrossNavigationService
    {
        Task<bool> Navigate<TViewModel>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel;

        Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter,
                ICrossBundle? presentationBundle = null, ICrossNavigateEventArgs? args = null,
                CancellationToken cancellationToken = default)
           where TViewModel : ICrossViewModel<TParameter, bool>;

        Task<TResult?> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null, 
                CancellationToken cancellationToken = default) 
            where TViewModel : ICrossViewModelResult<TResult>;

        Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter parameter, 
                ICrossBundle? presentationBundle = null, ICrossNavigateEventArgs? args = null, 
                CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter, TResult>;

        Task<bool> Close(ICrossViewModel viewModel, CancellationToken? cancellationToken = default(CancellationToken?));

        Task<bool> Close<TResult>(ICrossViewModelResult<TResult> viewModel, TResult result, 
            CancellationToken? cancellationToken = default(CancellationToken?));
    }
}
