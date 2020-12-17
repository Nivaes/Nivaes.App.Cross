namespace Nivaes.App.Cross.Mobele.Windows.Sample.Views
{
    using MvvmCross.Platforms.Uap.Presenters.Attributes;
    using MvvmCross.Platforms.Uap.Views;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Sample;

    [MvxViewFor(typeof(ModalViewModel))]
    [MvxDialogViewPresentation]
    public sealed partial class DialogView
        : DialogViewBase
    {
        public DialogView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class DialogViewBase
        : MvxWindowsContentDialog<ModalViewModel>
    {
    }
}
