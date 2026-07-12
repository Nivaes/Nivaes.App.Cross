using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI;

public class DialogService : IDialogService
{
    public async void Alert(string message, string title, string okbtnText)
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "Ok"
        };

        ContentDialogResult result = await dialog.ShowAsync();
    }
}
