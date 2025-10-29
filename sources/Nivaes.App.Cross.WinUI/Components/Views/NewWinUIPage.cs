namespace Nivaes.App.Cross.WinUI.Components.Views
{
    using Microsoft.UI.Xaml.Controls;

    public abstract class NewWinUIPage<TViewModel>
         : Page, ICrossView
        where TViewModel : class, ICrossViewModel
    {
    }
}
