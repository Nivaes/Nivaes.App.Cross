namespace Nivaes.App.Cross
{
    /// <summary>
    ///     Extension of MvxViewModelInstanceRequest with a target.
    /// </summary>
    public class CrossViewModelInstanceRequestWithSource : ViewModelRequest
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CrossViewModelInstanceRequestWithSource"/>
        /// </summary>
        /// <param name="viewModelType">The viewmodel type.</param>
        /// <param name="source">The instance of the viewmodel which is the source of the request.</param>
        public CrossViewModelInstanceRequestWithSource(Type viewModelType, ICrossViewModel source)
                : base(viewModelType)
        {
            this.Source = source;
        }

        /// <summary>
        ///     The instance of the viewmodel which is the source of the request.
        /// </summary>
        public ICrossViewModel Source { get; }
    }
}
