namespace Nivaes.App.Cross
{
    public interface ICrossUIKitViewModelRequest 
        : ICrossViewModelRequest
    {
        public string StoryboardName { get; }
    }
}
