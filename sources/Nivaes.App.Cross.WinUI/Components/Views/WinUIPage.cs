namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Xaml.Controls;

    public abstract class WinUIPage<TViewModel>
        : Page, IView
        where TViewModel : class, IViewModel
    {
    }
}
