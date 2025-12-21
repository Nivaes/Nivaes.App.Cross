namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICrossNavigationFacade
    {
        Task<CrossViewModelRequest> BuildViewModelRequest(string url, IDictionary<string, string> currentParameters);
    }
}
