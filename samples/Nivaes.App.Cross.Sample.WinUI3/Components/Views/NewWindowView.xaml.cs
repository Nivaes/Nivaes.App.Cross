namespace Nivaes.App.Cross.Sample.WinUI
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.WinUI;

    public sealed partial class NewWindowView 
        : NewWindowPage
    {
        public NewWindowView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class NewWindowPage 
        : CrpssWindowsPage<RootViewModel>
    {
    }
}
