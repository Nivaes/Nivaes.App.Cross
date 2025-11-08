namespace Nivaes.App.Cross.Sample.WinUI
{
    using Microsoft.UI.Xaml;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.WinUI;

    public sealed partial class RootView 
        : RootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RootViewPage 
        : CrpssWindowsPage<RootViewModel>
    {
    }
}
