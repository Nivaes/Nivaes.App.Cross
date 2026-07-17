namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BaseDetailViewController<TViewModel>
        : BaseViewController<TViewModel>
         where TViewModel : class, IBaseDetailViewModel
    {
        public BaseDetailViewController()
            : base()
        {

        }

        protected BaseDetailViewController(IntPtr handle)
            : base(handle)
        {
        }
    }
}
