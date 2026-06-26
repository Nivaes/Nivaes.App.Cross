using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

[MvxViewFor(typeof(ModalViewModel))]
[MvxDialogViewPresentation]
public sealed partial class DialogView : DialogViewBase
{
    public DialogView()
    {
        this.InitializeComponent();
    }
}

public abstract class DialogViewBase : CrossWindowsContentDialog<ModalViewModel>
{
}
