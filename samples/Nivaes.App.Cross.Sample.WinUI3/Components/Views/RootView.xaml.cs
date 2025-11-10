namespace Nivaes.App.Cross.Sample.WinUI3
{
    using Microsoft.UI.Xaml;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.WinUI3;

    public sealed partial class RootView 
        : RootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RootViewPage 
        : CrossWindowsPage<RootViewModel>
    {
    }
}
