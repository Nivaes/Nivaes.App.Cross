namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base master view controller </summary>
    public abstract class BaseMasterViewController<TViewModel>
        : BaseViewController<TViewModel>
          where TViewModel : MasterViewModel
    {
        public BaseMasterViewController()
            : base()
        {

        }

        protected BaseMasterViewController(IntPtr handle)
            : base(handle)
        {
        }
    }
}
