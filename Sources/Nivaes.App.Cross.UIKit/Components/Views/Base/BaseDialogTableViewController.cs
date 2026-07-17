namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base table view controller </summary>
    public abstract class BaseDialogTableViewController<TViewModel>
        : BaseTableViewController<TViewModel>
        where TViewModel : IDialogViewModel
    {
        protected virtual NavigationIconType NavigationIconType => NavigationIconType.Back;

        public BaseDialogTableViewController()
            : base()
        {
        }

        protected BaseDialogTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.TableView.TableFooterView = new UIView();

            if (NavigationIconType == NavigationIconType.Back)
            {
                base.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("ic_back"), UIBarButtonItemStyle.Plain, (object sender, EventArgs e) =>
                {
                    DialogClose();
                });
            }
            else if (NavigationIconType == NavigationIconType.Cancel)
            {
                base.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (object sender, EventArgs e) =>
                {
                    DialogClose();
                });
            }
        }

        private async void DialogClose()
        {
            await ViewModel.CloseCommand.ExecuteAsync().ConfigureAwait(false);
        }
    }
}
