
namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    public abstract class CrossViewPresentation : ICrossViewPresentation
    {
        public abstract Task<bool> ShowView(Type viewType, ICrossViewModelRequest request);

        public abstract Task<bool> CloseView(ICrossViewModel request);
    }
}
