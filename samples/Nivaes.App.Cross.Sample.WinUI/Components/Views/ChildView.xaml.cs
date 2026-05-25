using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

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
