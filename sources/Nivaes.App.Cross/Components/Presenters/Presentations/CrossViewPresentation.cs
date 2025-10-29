
namespace Nivaes.App.Cross.Presenters
{
    public abstract class CrossViewPresentation : ICrossViewPresentation
    {
        public abstract Task<bool> ShowView(Type viewType, ICrossViewModelRequest request);

        public abstract Task<bool> CloseView(ICrossViewModel request);
    }
}
