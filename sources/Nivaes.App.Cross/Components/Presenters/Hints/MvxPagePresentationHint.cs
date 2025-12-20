namespace MvvmCross.Presenters.Hints
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, CrossBundle body) : base(body)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, IDictionary<string, string> hints) : this(viewModel, new CrossBundle(hints))
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type ViewModel { get; }
    }
}
