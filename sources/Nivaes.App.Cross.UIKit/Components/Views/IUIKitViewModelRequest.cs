namespace Nivaes.App.Cross
{
    public interface IUIKitViewModelRequest 
        : ICrossViewModelRequest
    {
        public string StoryboardName { get; }
    }
}
