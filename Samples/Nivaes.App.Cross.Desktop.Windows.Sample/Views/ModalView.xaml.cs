namespace Nivaes.App.Cross.Desktop.Windows.Wpf.Sample
{
    using MvvmCross.Platforms.Wpf.Presenters.Attributes;

    [MvxWindowPresentation(Identifier = nameof(ModalView), Modal = true)]
    public partial class ModalView
    {
        public ModalView()
        {
            InitializeComponent();
        }
    }
}
