namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BaseTablePagerViewController<TViewModel>
        : MvxTableViewController<TViewModel>, IMvxTabBarItemViewController
        where TViewModel : BasePagerViewModel
    {
        public virtual string TabName => base.Title;
        public virtual string TabIconName { get; }
        public virtual string TabSelectedIconName { get; }

        public BaseTablePagerViewController()
            : base()
        {
        }

        protected BaseTablePagerViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.TableView.TableFooterView = new UIView();
        }
    }
}
