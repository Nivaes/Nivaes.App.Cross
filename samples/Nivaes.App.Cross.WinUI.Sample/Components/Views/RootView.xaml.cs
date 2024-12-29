namespace Nivaes.App.Cross.WinUI.Sample
{
    using Microsoft.UI.Xaml;
    using Nivaes.App.Cross.Sample;

    public sealed partial class RootView : RootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked view";
        }
    }

    public abstract class RootViewPage : WinUIPage<RootViewModel>
    {
    }
}
