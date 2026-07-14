using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI;

public class DialogViewPresentationAttribute : BasePresentationAttribute
{
    public DialogViewPresentationAttribute()
    {
    }

    public ContentDialogPlacement Placement { get; set; }
}
