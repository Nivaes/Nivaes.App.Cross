using Nivaes.App.Cross.Sample;
using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

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
