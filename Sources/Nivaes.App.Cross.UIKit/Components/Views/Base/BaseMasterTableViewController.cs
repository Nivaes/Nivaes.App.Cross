namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base master table view controller </summary>
    public abstract class BaseMasterTableViewController<TViewModel>
        : BaseTableViewController<TViewModel>
        where TViewModel : IMasterViewModel
    {
        #region Constructor
        public BaseMasterTableViewController()
            : base()
        {
        }

        protected BaseMasterTableViewController(IntPtr handle)
            : base(handle)
        {
        }
        #endregion
    }
}
