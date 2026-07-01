using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI;

public class MvxDialogViewPresentationAttribute : CrossBasePresentationAttribute
{
    public MvxDialogViewPresentationAttribute()
    {
    }

    public ContentDialogPlacement Placement { get; set; }
}
