namespace Playground.WinUi.Views
{
    using MvvmCross.Platforms.WinUi.Presenters.Attributes;
    using MvvmCross.Platforms.WinUi.Views;
    using Nivaes.App.Cross;
    using Playground.Core.ViewModels;

    [MvxViewFor(typeof(ModalViewModel))]
    [MvxDialogViewPresentation]
    public sealed partial class DialogView : DialogViewBase
    {
        public DialogView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class DialogViewBase : MvxWindowsContentDialog<ModalViewModel>
    {
    }
}
