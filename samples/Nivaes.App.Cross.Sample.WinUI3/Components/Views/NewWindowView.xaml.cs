namespace Nivaes.App.Cross.Sample.WinUI3
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.WinUI3;

    public sealed partial class NewWindowView 
        : NewWindowPage
    {
        public NewWindowView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class NewWindowPage 
        : CrossWindowsPage<NewWindowViewModel>
    {
    }
}
