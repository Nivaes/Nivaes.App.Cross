using Microsoft.UI.Xaml.Controls;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.WinUI3;

public class MvxDialogViewPresentationAttribute : CrossBasePresentationAttribute
{
    public MvxDialogViewPresentationAttribute()
    {
    }

    public ContentDialogPlacement Placement { get; set; }
}
