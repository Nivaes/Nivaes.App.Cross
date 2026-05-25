using Microsoft.UI.Xaml;

namespace Nivaes.App.Cross.WinUI;

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
