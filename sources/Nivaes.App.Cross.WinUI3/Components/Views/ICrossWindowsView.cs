using Microsoft.UI.Xaml;

namespace Nivaes.App.Cross.WinUI3;

public interface ICrossWindowsView
    : ICrossView
{
    UIElement Content { get; set; }

    void ClearBackStack();
}

public interface ICrossWindowsView<TViewModel>
        : ICrossWindowsView
        , ICrossView<TViewModel> 
    where TViewModel : class, ICrossViewModel
{
}
