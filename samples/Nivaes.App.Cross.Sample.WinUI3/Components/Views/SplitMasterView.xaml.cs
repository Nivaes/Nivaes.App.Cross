using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Pane)]
    public sealed partial class SplitMasterView : CrossWindowsPage
    {
        public SplitMasterView()
        {
            this.InitializeComponent();
        }
    }
}
