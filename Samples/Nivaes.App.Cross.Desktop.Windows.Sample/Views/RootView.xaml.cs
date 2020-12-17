namespace Nivaes.App.Cross.Desktop.Windows.Wpf.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Sample;

    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}
