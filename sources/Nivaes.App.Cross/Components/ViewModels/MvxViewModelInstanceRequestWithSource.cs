namespace MvvmCross.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    /// <summary>
    ///     Extension of MvxViewModelInstanceRequest with a target.
    /// </summary>
    public class MvxViewModelInstanceRequestWithSource : MvxViewModelInstanceRequest
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MvxViewModelInstanceRequestWithSource"/>
        /// </summary>
        /// <param name="viewModelType">The viewmodel type.</param>
        /// <param name="source">The instance of the viewmodel which is the source of the request.</param>
        public MvxViewModelInstanceRequestWithSource(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossViewModel source)
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
