namespace Nivaes.App.Cross.UIKitLib
{

    /// <summary> A base view controller </summary>
    public abstract class BaseDefaultDetailViewController<TViewModel>
        : MvxViewController<TViewModel>
        where TViewModel : BaseDefaultDetailViewModel
    {
        public BaseDefaultDetailViewController()
            : base()
        {
        }

        protected BaseDefaultDetailViewController(IntPtr handle)
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
