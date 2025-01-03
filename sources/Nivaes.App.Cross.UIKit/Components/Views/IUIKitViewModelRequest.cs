namespace Nivaes.App.Cross
{
    public interface IUIKitViewModelRequest 
        : IViewModelRequest
    {
        public string StoryboardName { get; }
    }
}
