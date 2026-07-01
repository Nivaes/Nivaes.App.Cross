using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

public sealed partial class ChildView : ChildViewPagePage
{
    public ChildView()
    {
        this.InitializeComponent();
    }
}

public abstract class ChildViewPagePage
    : CrossWindowsPage<ChildViewModel>
{
}
