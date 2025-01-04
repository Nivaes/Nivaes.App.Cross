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

        public string Cosa {get; set;} = "Prueba cosa2";

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked view";

            ViewModel!.Title = "Nuevo título";
        }
    }

    public abstract class RootViewPage 
        : CrossPage<RootViewModel>
    {
    }
}
