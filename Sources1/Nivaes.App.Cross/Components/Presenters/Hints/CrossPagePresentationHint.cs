namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossPagePresentationHint
        : CrossPresentationHint
    {
        public CrossPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel)
        {
            ViewModel = viewModel;
        }

        public CrossPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, CrossBundle body) : base(body)
        {
            ViewModel = viewModel;
        }

        public CrossPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, IDictionary<string, string> hints) : this(viewModel, new CrossBundle(hints))
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type ViewModel { get; }
    }
}
