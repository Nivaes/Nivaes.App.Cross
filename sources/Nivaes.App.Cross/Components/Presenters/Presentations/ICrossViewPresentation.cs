namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    public interface ICrossViewPresentation
    {
        Task<bool> ShowView(Type viewType, ICrossViewModelRequest request);

        Task<bool> CloseView(ICrossViewModel request);
    }
}
