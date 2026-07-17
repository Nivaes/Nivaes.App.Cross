namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BaseDialogViewController<TViewModel>
        : BaseViewController<TViewModel>
        where TViewModel : IDialogViewModel
    {
        protected virtual NavigationIconType NavigationIconType { get; } = NavigationIconType.Back;

        public BaseDialogViewController()
            : base()
        {
        }

        protected BaseDialogViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (NavigationIconType == NavigationIconType.Back)
            {
                base.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("ic_back")!, UIBarButtonItemStyle.Plain, (object? sender, EventArgs e) =>
                {
                    DialogClose();
                });
            }
            else if (NavigationIconType == NavigationIconType.Cancel)
            {
                base.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (object? sender, EventArgs e) =>
                {
                    DialogClose();
                });
            }
        }

        private async void DialogClose()
        {
            await ViewModel!.CloseCommand.ExecuteAsync().ConfigureAwait(false);
        }
    }
}
