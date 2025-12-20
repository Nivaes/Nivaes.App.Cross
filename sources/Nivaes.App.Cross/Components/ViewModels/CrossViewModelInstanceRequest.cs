// ToDo: ¿Es necesario?

namespace Nivaes.App.Cross
{ 
    [Obsolete]
    public record class CrossViewModelInstanceRequest<TViewModel> :
        CrossViewModelRequest<TViewModel>
        where TViewModel : ICrossViewModel
    {
        public ICrossViewModel? ViewModelInstance { get; set; }

        public CrossViewModelInstanceRequest(TViewModel viewModel)
            : base(viewModel)
        {
        }

        public CrossViewModelInstanceRequest(TViewModel viewModel, ICrossViewModel viewModelInstance)
            : base(viewModel, null, null)
        {
            ViewModelInstance = viewModelInstance;
        }
    }
}
