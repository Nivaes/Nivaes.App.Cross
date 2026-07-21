namespace Nivaes.App.Cross
{
    internal interface IViewModelRequestSource
    {
        public ICrossViewModel Source { get; }
    }

    /// <summary>
    ///     Extension of MvxViewModelInstanceRequest with a target.
    /// </summary>
    internal record ViewModelRequestSource<TViewModel> 
        : ViewModelRequest<TViewModel>, IViewModelRequestSource
        where TViewModel : ICrossViewModel
    {
        /// <summary>
        ///     The instance of the viewmodel which is the source of the request.
        /// </summary>
        public ICrossViewModel Source { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CrossViewModelInstanceRequestWithSource"/>
        /// </summary>
        /// <param name="viewModelType">The viewmodel type.</param>
        /// <param name="source">The instance of the viewmodel which is the source of the request.</param>
        public ViewModelRequestSource(ICrossViewModel source)
                : base()
        {
            this.Source = source;
        }
    }
}
