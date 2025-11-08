namespace Nivaes.App.Cross
{
    public record UIKitViewModelRequest<TViewModel>
        : CrossViewModelRequest<TViewModel>,
            ICrossUIKitViewModelRequest
        where TViewModel : ICrossViewModel
    {
        public UIKitViewModelRequest(TViewModel viewModel, string storyboardName)
            : base(viewModel)
        {
            StoryboardName = storyboardName;
        }

        public string StoryboardName { get; }
    }
}
