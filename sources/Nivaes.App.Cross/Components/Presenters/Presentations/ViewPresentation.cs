
namespace Nivaes.App.Cross.Presenters
{
    public abstract class ViewPresentation : IViewPresentation
    {
        public abstract Task<bool> ShowView(Type viewType, IViewModelRequest request);

        public abstract Task<bool> CloseView(IViewModel request);
    }
}
