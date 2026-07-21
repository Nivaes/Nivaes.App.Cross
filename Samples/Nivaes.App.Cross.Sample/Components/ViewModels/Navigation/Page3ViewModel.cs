using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class Page3ViewModel
    : CrossNavigationViewModel
{
    public Page3ViewModel(ILogger<Page3ViewModel> logger)
        : base(logger)
    {
    }
}
