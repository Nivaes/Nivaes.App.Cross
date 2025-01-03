namespace Nivaes.App.Cross
{
    public record UIKitViewModelRequest<TViewModel>
        : ViewModelRequest<TViewModel>,
            IUIKitViewModelRequest
        where TViewModel : IViewModel
    {
        public UIKitViewModelRequest(TViewModel viewModel, string storyboardName)
            : base(viewModel)
        {
            StoryboardName = storyboardName;
        }

        public string StoryboardName { get; }
    }
}
