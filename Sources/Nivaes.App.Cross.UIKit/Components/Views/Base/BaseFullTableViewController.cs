namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base full view controller </summary>
    public abstract class BaseFullTableViewController<TViewModel>
        : BaseTableViewController<TViewModel>
        where TViewModel : FullViewModel
    {
        public BaseFullTableViewController()
            : base()
        {
        }

        protected BaseFullTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.EdgesForExtendedLayout = UIKit.UIRectEdge.None;

            base.ViewDidLoad();
        }
    }
}
