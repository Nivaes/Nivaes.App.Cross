namespace Nivaes.App.Cross.WinUI.Sample
{
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.WinUI.Components.Views;

    public sealed partial class NewWindowView : NewWindowPage
    {
        public NewWindowView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class NewWindowPage : NewWinUIPage<RootViewModel>
    {
    }
}
