namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base master table view controller </summary>
    public abstract class BaseDetailTableViewController<TViewModel>
        : BaseTableViewController<TViewModel>
        where TViewModel : BaseViewModel
    {
        #region Constructor
        public BaseDetailTableViewController()
            : base()
        {
        }

        protected BaseDetailTableViewController(IntPtr handle)
            : base(handle)
        {
        }
        #endregion
    }
}
