using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class Page2ViewModel
    : CrossNavigationViewModel
{
    public Page2ViewModel(ILogger<Page2ViewModel> loggger)
        : base(loggger)
    {
    }
}
