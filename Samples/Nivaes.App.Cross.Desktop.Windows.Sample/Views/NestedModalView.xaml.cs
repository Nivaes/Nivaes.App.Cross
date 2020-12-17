namespace Nivaes.App.Cross.Desktop.Windows.Wpf.Sample
{
    using MvvmCross.Platforms.Wpf.Presenters.Attributes;

    [MvxContentPresentation(WindowIdentifier = nameof(ModalView))]
    public partial class NestedModalView
    {
        public NestedModalView()
        {
            InitializeComponent();
        }
    }
}
